namespace Application.Dtos.Profile
{
    public record UserPostDto(
        string Id,
        string Title,
        string Content,
        string? ImageUrl,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        int ViewCount,
        int UpvoteCount,
        int DownvoteCount,
        int CommentCount);
}