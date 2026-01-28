using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IDesignStyleRepository : IGenericRepository<DesignStyle>
    {
        Task<DesignStyle?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
