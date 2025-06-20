using Domain.Entities;

namespace Domain.Interfaces;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsForTaskAsync(Guid taskId);
}