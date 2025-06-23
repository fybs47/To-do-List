using Application.DTOs.Comments;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class CommentService : ICommentService
{
    public Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto, Guid authorId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentDto>> GetCommentsForTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDto> UpdateCommentAsync(Guid commentId, UpdateCommentDto updateCommentDto, Guid userId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}