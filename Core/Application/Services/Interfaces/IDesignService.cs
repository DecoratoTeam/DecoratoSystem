using Application.Dtos;
using Application.Dtos.Design;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface IDesignService
    {
        Task<GeneralResponseDto<DesignImageResponseDto>> GenerateFromTextAsync(string prompt, CancellationToken cancellationToken);
        Task<GeneralResponseDto<DesignImageResponseDto>> TransformAsync(IFormFile image, string prompt, CancellationToken cancellationToken);
    }
}