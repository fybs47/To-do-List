using Application.DTOs.Shared;
using Application.DTOs.Users;
using Application.Exceptions;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching user by ID: {UserId}", userId);
            
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            
            return _mapper.Map<UserDto>(user);
        }

        public async Task<PagedResponse<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all users, page: {Page}, pageSize: {PageSize}", page, pageSize);
            
            var users = await _userRepository.GetAllAsync(cancellationToken);
            var totalCount = users.Count();
            var pagedUsers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            
            return new PagedResponse<UserDto>
            {
                Items = _mapper.Map<IEnumerable<UserDto>>(pagedUsers),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto, Guid currentUserId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating user {UserId} by {CurrentUserId}", userId, currentUserId);
            
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken) 
                ?? throw new NotFoundException("User not found");
            
            // Только сам пользователь или администратор может обновлять данные
            if (user.Id != currentUserId)
            {
                var currentUser = await _userRepository.GetByIdAsync(currentUserId, cancellationToken);
                if (currentUser?.Role != Role.Admin)
                {
                    throw new ForbiddenException("You don't have permission to update this user");
                }
            }

            _mapper.Map(updateUserDto, user);
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<UserDto>(user);
        }

        public async Task ToggleUserStatusAsync(Guid userId, Guid currentUserId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Toggling status for user {UserId} by {CurrentUserId}", userId, currentUserId);
            
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken) 
                ?? throw new NotFoundException("User not found");
            
            // Только администратор может менять статус
            var currentUser = await _userRepository.GetByIdAsync(currentUserId, cancellationToken);
            if (currentUser?.Role != Role.Admin)
            {
                throw new ForbiddenException("You don't have permission to change user status");
            }

            user.IsActive = !user.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }
    }
}