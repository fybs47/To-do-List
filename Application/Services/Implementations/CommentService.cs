using Application.DTOs.Comments;
using Application.Exceptions;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITaskItemRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        public CommentService(
            ICommentRepository commentRepository,
            ITaskItemRepository taskRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto, Guid authorId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating comment by user: {UserId}", authorId);
            
            var author = await _userRepository.GetByIdAsync(authorId, cancellationToken) 
                ?? throw new NotFoundException("User not found");
            
            var task = await _taskRepository.GetByIdAsync(createCommentDto.TaskId, cancellationToken) 
                ?? throw new NotFoundException("Task not found");

            var comment = new Comment
            {
                Text = createCommentDto.Text,
                AuthorId = authorId,
                TaskId = createCommentDto.TaskId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment, cancellationToken);
            await _commentRepository.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching comment by ID: {CommentId}", commentId);
            
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }
            
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsForTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching comments for task: {TaskId}", taskId);
            
            var comments = await _commentRepository.GetCommentsForTaskAsync(taskId, cancellationToken);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto> UpdateCommentAsync(Guid commentId, UpdateCommentDto updateCommentDto, Guid userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating comment {CommentId} by user {UserId}", commentId, userId);
            
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken) 
                ?? throw new NotFoundException("Comment not found");

            if (comment.AuthorId != userId)
            {
                throw new ForbiddenException("You don't have permission to edit this comment");
            }

            comment.Text = updateCommentDto.Text;
            comment.UpdatedAt = DateTime.UtcNow;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task DeleteCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting comment {CommentId} by user {UserId}", commentId, userId);
            
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken) 
                ?? throw new NotFoundException("Comment not found");

            if (comment.AuthorId != userId)
            {
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user?.Role != Domain.Enums.Role.Admin)
                {
                    throw new ForbiddenException("You don't have permission to delete this comment");
                }
            }

            _commentRepository.Remove(comment);
            await _commentRepository.SaveChangesAsync(cancellationToken);
        }
    }
}