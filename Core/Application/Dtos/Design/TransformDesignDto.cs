using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Design
{
    public record TransformDesignDto(IFormFile Image, string Prompt);
}