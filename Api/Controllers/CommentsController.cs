using Application.DTOs.Comments;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(ICommentService commentService, ILogger<CommentsController> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(
        CreateCommentDto createCommentDto,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Comment creation by user {UserId} for task {TaskId}", 
            userId, createCommentDto.TaskId);
        var comment = await _commentService.CreateCommentAsync(createCommentDto, userId, cancellationToken);
        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Comment {CommentId} retrieval by user {UserId}", id, userId);
        var comment = await _commentService.GetCommentByIdAsync(id, cancellationToken);
        return Ok(comment);
    }

    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsForTask(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Comments retrieval for task {TaskId} by user {UserId}", taskId, userId);
        var comments = await _commentService.GetCommentsForTaskAsync(taskId, cancellationToken);
        return Ok(comments);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(
        Guid id,
        UpdateCommentDto updateCommentDto,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Comment {CommentId} update by user {UserId}", id, userId);
        var comment = await _commentService.UpdateCommentAsync(id, updateCommentDto, userId, cancellationToken);
        return Ok(comment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Comment {CommentId} deletion by user {UserId}", id, userId);
        await _commentService.DeleteCommentAsync(id, userId, cancellationToken);
        return NoContent();
    }
}