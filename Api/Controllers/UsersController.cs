// Api/Controllers/UsersController.cs
using Application.DTOs.Shared;
using Application.DTOs.Users;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        _logger.LogInformation("User {UserId} retrieval by admin {AdminId}", id, currentUserId);
        var user = await _userService.GetUserByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<UserDto>>> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = User.GetUserId();
        _logger.LogInformation("Users list retrieval by admin {AdminId}", currentUserId);
        var users = await _userService.GetAllUsersAsync(page, pageSize, cancellationToken);
        return Ok(users);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(
        Guid id,
        UpdateUserDto updateUserDto,
        CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        _logger.LogInformation("User {UserId} update by admin {AdminId}", id, currentUserId);
        var user = await _userService.UpdateUserAsync(id, updateUserDto, currentUserId, cancellationToken);
        return Ok(user);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ToggleUserStatus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        _logger.LogInformation("User {UserId} status toggle by admin {AdminId}", id, currentUserId);
        await _userService.ToggleUserStatusAsync(id, currentUserId, cancellationToken);
        return NoContent();
    }
}