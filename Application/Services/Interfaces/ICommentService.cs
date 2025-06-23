using Application.DTOs.Comments;

namespace Application.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto, Guid authorId, CancellationToken cancellationToken = default);
    Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentDto>> GetCommentsForTaskAsync(Guid taskId, CancellationToken cancellationToken = default);
    Task<CommentDto> UpdateCommentAsync(Guid commentId, UpdateCommentDto updateCommentDto, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default);
}