using Application.Dtos;
using Application.Dtos.Profile;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DecorteeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {
        private readonly IProfileService _profileService = profileService;

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Get current user profile
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseDto<ProfileResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<ProfileResponseDto>>> GetProfile(CancellationToken cancellationToken)
        {
            var result = await _profileService.GetProfileAsync(GetUserId(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Update profile (name, email, bio)
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(GeneralResponseDto<ProfileResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<ProfileResponseDto>>> UpdateProfile(
            [FromBody] UpdateProfileDto dto, CancellationToken cancellationToken)
        {
            var result = await _profileService.UpdateProfileAsync(GetUserId(), dto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Change password
        /// </summary>
        [HttpPut("password")]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<bool>>> ChangePassword(
            [FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
        {
            var result = await _profileService.ChangePasswordAsync(GetUserId(), dto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Update language preference
        /// </summary>
        [HttpPut("language")]
        [ProducesResponseType(typeof(GeneralResponseDto<ProfileResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<ProfileResponseDto>>> UpdateLanguage(
            [FromBody] UpdateLanguageDto dto, CancellationToken cancellationToken)
        {
            var result = await _profileService.UpdateLanguageAsync(GetUserId(), dto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Toggle dark mode
        /// </summary>
        [HttpPut("theme")]
        [ProducesResponseType(typeof(GeneralResponseDto<ProfileResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<ProfileResponseDto>>> UpdateTheme(
            [FromBody] UpdateThemeDto dto, CancellationToken cancellationToken)
        {
            var result = await _profileService.UpdateThemeAsync(GetUserId(), dto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Upload profile image
        /// </summary>
        [HttpPost("upload-image")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(GeneralResponseDto<ProfileResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<ProfileResponseDto>>> UploadProfileImage(
            IFormFile file, CancellationToken cancellationToken)
        {
            var result = await _profileService.UploadProfileImageAsync(GetUserId(), file, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Logout (client-side token discard)
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        public ActionResult<GeneralResponseDto<bool>> Logout()
        {
            // JWT is stateless — client discards token.
            return Ok(GeneralResponseDto<bool>.Success(true));
        }

        /// <summary>
        /// Get paginated posts for logged-in user
        /// </summary>
        [HttpGet("posts")]
        [ProducesResponseType(typeof(GeneralResponseDto<PaginatedResultDto<UserPostDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<PaginatedResultDto<UserPostDto>>>> GetMyPosts(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var result = await _profileService.GetMyPostsAsync(GetUserId(), page, pageSize, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Delete own post
        /// </summary>
        [HttpDelete("posts/{postId}")]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<bool>>> DeletePost(string postId, CancellationToken cancellationToken)
        {
            var result = await _profileService.DeletePostAsync(GetUserId(), postId, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get all ratings made by the logged-in user
        /// </summary>
        [HttpGet("ratings")]
        [ProducesResponseType(typeof(GeneralResponseDto<IEnumerable<UserRatingDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<IEnumerable<UserRatingDto>>>> GetMyRatings(CancellationToken cancellationToken)
        {
            var result = await _profileService.GetMyRatingsAsync(GetUserId(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Rate a post (1-5)
        /// </summary>
        [HttpPost("~/api/posts/{postId}/rate")]
        [ProducesResponseType(typeof(GeneralResponseDto<UserRatingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<UserRatingDto>>> RatePost(
            string postId, [FromBody] RatePostDto dto, CancellationToken cancellationToken)
        {
            var result = await _profileService.RatePostAsync(GetUserId(), postId, dto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get user statistics (total posts, total ratings, average rating)
        /// </summary>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(GeneralResponseDto<ProfileStatsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<ProfileStatsDto>>> GetStats(CancellationToken cancellationToken)
        {
            var result = await _profileService.GetStatsAsync(GetUserId(), cancellationToken);
            return Ok(result);
        }
    }
}