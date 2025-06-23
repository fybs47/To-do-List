using System.IO;
using Application.DTOs.Shared;
using Application.DTOs.Tasks;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskItemRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskItemRepository taskRepository,
            IUserRepository userRepository,
            ITaskHistoryRepository taskHistoryRepository,
            IMapper mapper,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _taskHistoryRepository = taskHistoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, Guid creatorId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating new task by user: {UserId}", creatorId);
            
            var creator = await _userRepository.GetByIdAsync(creatorId, cancellationToken) 
                ?? throw new NotFoundException("User not found");

            var task = _mapper.Map<TaskItem>(createTaskDto);
            task.CreatorId = creatorId;
            
            if (createTaskDto.AssigneeId.HasValue)
            {
                var assignee = await _userRepository.GetByIdAsync(createTaskDto.AssigneeId.Value, cancellationToken);
                if (assignee == null) throw new NotFoundException("Assignee not found");
                task.AssigneeId = assignee.Id;
            }

            await _taskRepository.AddAsync(task, cancellationToken);
            await _taskRepository.SaveChangesAsync(cancellationToken);

            await AddTaskHistoryAsync(task, "Created", null, null, creatorId, cancellationToken);
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching task by ID: {TaskId}", taskId);
            
            var task = await _taskRepository.GetByIdWithDetailsAsync(taskId, cancellationToken);
            if (task == null)
            {
                throw new NotFoundException("Task not found");
            }
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<PagedResponse<TaskDto>> GetTasksAsync(TaskFilterDto filter, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching tasks with filter");
            
            var query = _taskRepository.GetQueryable()
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .AsQueryable();

            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == (TaskStatus)filter.Status.Value);
            
            if (filter.Priority.HasValue)
                query = query.Where(t => t.Priority == filter.Priority.Value);
            
            if (filter.DueDateFrom.HasValue)
                query = query.Where(t => t.DueDate >= filter.DueDateFrom.Value);
            
            if (filter.DueDateTo.HasValue)
                query = query.Where(t => t.DueDate <= filter.DueDateTo.Value);
            
            if (filter.CreatorId.HasValue)
                query = query.Where(t => t.CreatorId == filter.CreatorId.Value);
            
            if (filter.AssigneeId.HasValue)
                query = query.Where(t => t.AssigneeId == filter.AssigneeId.Value);
            
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(t => 
                    t.Title.Contains(filter.SearchTerm) || 
                    t.Description.Contains(filter.SearchTerm));
            }

            query = filter.SortBy switch
            {
                "dueDate" => filter.SortDescending 
                    ? query.OrderByDescending(t => t.DueDate) 
                    : query.OrderBy(t => t.DueDate),
                "priority" => filter.SortDescending 
                    ? query.OrderByDescending(t => t.Priority) 
                    : query.OrderBy(t => t.Priority),
                _ => filter.SortDescending 
                    ? query.OrderByDescending(t => t.CreatedAt) 
                    : query.OrderBy(t => t.CreatedAt)
            };

            var totalCount = await query.CountAsync(cancellationToken);
            var tasks = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResponse<TaskDto>
            {
                Items = _mapper.Map<List<TaskDto>>(tasks),
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<TaskDto> UpdateTaskAsync(Guid taskId, UpdateTaskDto updateTaskDto, Guid userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating task {TaskId} by user {UserId}", taskId, userId);
            
            var task = await _taskRepository.GetByIdWithDetailsAsync(taskId, cancellationToken) 
                ?? throw new NotFoundException("Task not found");

            if (task.CreatorId != userId && task.AssigneeId != userId)
            {
                throw new ForbiddenException("You don't have permission to update this task");
            }

            var historyEntries = new List<TaskHistory>();
            
            if (updateTaskDto.Title != null && task.Title != updateTaskDto.Title)
            {
                historyEntries.Add(CreateHistoryEntry(task, "Title", task.Title, updateTaskDto.Title, userId));
                task.Title = updateTaskDto.Title;
            }
            
            if (updateTaskDto.Description != null && task.Description != updateTaskDto.Description)
            {
                historyEntries.Add(CreateHistoryEntry(task, "Description", task.Description, updateTaskDto.Description, userId));
                task.Description = updateTaskDto.Description;
            }
            
            if (updateTaskDto.Status.HasValue && task.Status != (TaskStatus)updateTaskDto.Status.Value)
            {
                historyEntries.Add(CreateHistoryEntry(task, "Status", task.Status.ToString(), updateTaskDto.Status.Value.ToString(), userId));
                task.Status = (TaskStatus)updateTaskDto.Status.Value;
            }
            
            if (updateTaskDto.Priority.HasValue && task.Priority != updateTaskDto.Priority.Value)
            {
                historyEntries.Add(CreateHistoryEntry(task, "Priority", task.Priority.ToString(), updateTaskDto.Priority.Value.ToString(), userId));
                task.Priority = updateTaskDto.Priority.Value;
            }
            
            if (updateTaskDto.DueDate.HasValue && task.DueDate != updateTaskDto.DueDate.Value)
            {
                historyEntries.Add(CreateHistoryEntry(task, "DueDate", task.DueDate?.ToString() ?? "", updateTaskDto.DueDate.Value.ToString(), userId));
                task.DueDate = updateTaskDto.DueDate.Value;
            }
            
            if (updateTaskDto.AssigneeId.HasValue && task.AssigneeId != updateTaskDto.AssigneeId.Value)
            {
                var newAssignee = await _userRepository.GetByIdAsync(updateTaskDto.AssigneeId.Value, cancellationToken);
                if (newAssignee == null) throw new NotFoundException("Assignee not found");
                
                historyEntries.Add(CreateHistoryEntry(task, "Assignee", 
                    task.AssigneeId?.ToString() ?? "", 
                    updateTaskDto.AssigneeId.Value.ToString(), 
                    userId));
                
                task.AssigneeId = updateTaskDto.AssigneeId.Value;
            }

            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync(cancellationToken);

            if (historyEntries.Any())
            {
                await _taskHistoryRepository.AddRangeAsync(historyEntries, cancellationToken);
                await _taskHistoryRepository.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<TaskDto>(task);
        }

        public async Task DeleteTaskAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting task {TaskId} by user {UserId}", taskId, userId);
            
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken) 
                ?? throw new NotFoundException("Task not found");

            if (task.CreatorId != userId)
            {
                throw new ForbiddenException("You don't have permission to delete this task");
            }

            _taskRepository.Remove(task);
            await _taskRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<TaskDto> ChangeTaskStatusAsync(Guid taskId, 
            ChangeTaskStatusDto statusDto, 
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (statusDto == null)
            {
                throw new ArgumentNullException(nameof(statusDto));
            }

            _logger.LogInformation("Changing status for task {TaskId} by user {UserId}", taskId, userId);
    
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken) 
                       ?? throw new NotFoundException("Task not found");

            if (task.CreatorId != userId && task.AssigneeId != userId)
            {
                throw new ForbiddenException("You don't have permission to change status of this task");
            }

            if (task.Status == statusDto.Status)
            {
                return _mapper.Map<TaskDto>(task);
            }

            await AddTaskHistoryAsync(task, 
                "Status", 
                task.Status.ToString(), 
                statusDto.Status.ToString(), 
                userId, 
                cancellationToken);

            task.Status = statusDto.Status;
            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync(cancellationToken);
    
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> AssignTaskAsync(Guid taskId, Guid assigneeId, Guid userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Assigning task {TaskId} to user {AssigneeId} by user {UserId}", taskId, assigneeId, userId);
            
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken) 
                ?? throw new NotFoundException("Task not found");
            
            var assignee = await _userRepository.GetByIdAsync(assigneeId, cancellationToken) 
                ?? throw new NotFoundException("Assignee not found");

            if (task.CreatorId != userId)
            {
                throw new ForbiddenException("You don't have permission to assign this task");
            }

            if (task.AssigneeId == assigneeId)
            {
                return _mapper.Map<TaskDto>(task);
            }

            await AddTaskHistoryAsync(task, "Assignee", 
                task.AssigneeId?.ToString() ?? "", 
                assigneeId.ToString(), 
                userId, cancellationToken);

            task.AssigneeId = assigneeId;
            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<IEnumerable<TaskHistoryDto>> GetTaskHistoryAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching history for task: {TaskId}", taskId);
            
            var histories = await _taskHistoryRepository.GetHistoryForTaskAsync(taskId, cancellationToken);
            return _mapper.Map<IEnumerable<TaskHistoryDto>>(histories);
        }

        public async Task<Stream> ExportTasksToCsvAsync(TaskFilterDto filter, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Exporting tasks to CSV");
            
            var tasks = (await GetTasksAsync(filter, cancellationToken)).Items;
            var csvData = CsvExportHelper.ExportToCsv(tasks);
            return new MemoryStream(csvData);
        }

        private async Task AddTaskHistoryAsync(
            TaskItem task, 
            string fieldName, 
            string? oldValue, 
            string? newValue, 
            Guid userId,
            CancellationToken cancellationToken)
        {
            var history = new TaskHistory
            {
                TaskId = task.Id,
                ChangedField = fieldName,
                OldValue = oldValue ?? string.Empty,
                NewValue = newValue ?? string.Empty,
                ChangedByUserId = userId,
                ChangedAt = DateTime.UtcNow
            };

            await _taskHistoryRepository.AddAsync(history, cancellationToken);
            await _taskHistoryRepository.SaveChangesAsync(cancellationToken);
        }

        private TaskHistory CreateHistoryEntry(
            TaskItem task, 
            string fieldName, 
            string oldValue, 
            string newValue, 
            Guid userId)
        {
            return new TaskHistory
            {
                TaskId = task.Id,
                ChangedField = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedByUserId = userId,
                ChangedAt = DateTime.UtcNow
            };
        }
    }
}