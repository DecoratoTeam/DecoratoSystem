using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IRoomTypeRepository : IGenericRepository<RoomType>
    {
        Task<RoomType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
