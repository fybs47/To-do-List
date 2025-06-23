using Domain.Entities;
using Domain.Interfaces;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsForTaskAsync(Guid taskId, CancellationToken cancellationToken = default);
}