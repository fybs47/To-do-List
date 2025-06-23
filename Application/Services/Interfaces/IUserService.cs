using Application.DTOs.Shared;
using Application.DTOs.Users;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResponse<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto, Guid currentUserId, CancellationToken cancellationToken = default);
    Task ToggleUserStatusAsync(Guid userId, Guid currentUserId, CancellationToken cancellationToken = default);
}