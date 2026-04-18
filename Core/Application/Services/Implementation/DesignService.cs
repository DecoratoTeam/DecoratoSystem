using Application.Dtos;
using Application.Dtos.Design;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class DesignService(IHttpClientFactory httpClientFactory, ILogger<DesignService> logger) : IDesignService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("DesignClient");
        private readonly ILogger<DesignService> _logger = logger;

        public async Task<GeneralResponseDto<DesignImageResponseDto>> GenerateFromTextAsync(string prompt, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return GeneralResponseDto<DesignImageResponseDto>.Fail(ErrorType.InvalidCredentials, "Prompt is required");

            try
            {
                using var response = await _httpClient.PostAsJsonAsync("img2img", new { prompt }, cancellationToken);
                return await HandleImageResponseAsync(response, "Text to image request failed", cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Text to image request failed");
                return GeneralResponseDto<DesignImageResponseDto>.Fail(ErrorType.ServerError, "Failed to generate image");
            }
        }

        public async Task<GeneralResponseDto<DesignImageResponseDto>> TransformAsync(IFormFile image, string prompt, CancellationToken cancellationToken)
        {
            if (image is null || image.Length == 0)
                return GeneralResponseDto<DesignImageResponseDto>.Fail(ErrorType.InvalidCredentials, "Image file is required");

            if (string.IsNullOrWhiteSpace(prompt))
                return GeneralResponseDto<DesignImageResponseDto>.Fail(ErrorType.InvalidCredentials, "Prompt is required");

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                return GeneralResponseDto<DesignImageResponseDto>.Fail(
                    ErrorType.InvalidCredentials,
                    "Only jpg, jpeg, png, webp are allowed");
            }

            try
            {
                using var form = new MultipartFormDataContent();
                using var imageContent = new StreamContent(image.OpenReadStream());
                if (!string.IsNullOrWhiteSpace(image.ContentType))
                {
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                }

                form.Add(imageContent, "image", image.FileName);
                form.Add(new StringContent(prompt), "prompt");

                using var response = await _httpClient.PostAsync("generate", form, cancellationToken);
                return await HandleImageResponseAsync(response, "Image to image request failed", cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Image to image request failed");
                return GeneralResponseDto<DesignImageResponseDto>.Fail(ErrorType.ServerError, "Failed to generate image");
            }
        }

        private async Task<GeneralResponseDto<DesignImageResponseDto>> HandleImageResponseAsync(
            HttpResponseMessage response,
            string errorMessage,
            CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("{ErrorMessage}. Status: {StatusCode}. Body: {Body}", errorMessage, (int)response.StatusCode, body);
                return GeneralResponseDto<DesignImageResponseDto>.Fail(ErrorType.ServerError, "Failed to generate image");
            }

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            if (bytes.Length == 0)
            {
                _logger.LogError("{ErrorMessage}. Empty response body.", errorMessage);
                return GeneralResponseDto<DesignImageResponseDto>.Fail(ErrorType.ServerError, "Failed to generate image");
            }

            var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/png";
            var base64 = Convert.ToBase64String(bytes);

            return GeneralResponseDto<DesignImageResponseDto>.Success(new DesignImageResponseDto(contentType, base64));
        }
    }
}