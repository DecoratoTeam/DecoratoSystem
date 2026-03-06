using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IShowcaseDesignRepository : IGenericRepository<ShowcaseDesign>
    {
        Task<IEnumerable<ShowcaseDesign>> GetByRoomTypeAsync(string roomTypeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ShowcaseDesign>> GetByStyleAsync(string styleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ShowcaseDesign>> GetPopularAsync(int take = 10, CancellationToken cancellationToken = default);
        Task<IEnumerable<ShowcaseDesign>> GetTrendingAsync(int take = 10, CancellationToken cancellationToken = default);
        Task<IEnumerable<ShowcaseDesign>> GetFilteredAsync(string? roomTypeId, string? styleId, bool? isPopular, bool? isTrending, CancellationToken cancellationToken = default);
        Task<ShowcaseDesign?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default);
    }
}
