using Application.Dtos.Profile;
using Domain.Entities;
using Mapster;

namespace DecorteeSystem.Mappings
{
    public class ProfileMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, ProfileResponseDto>()
                .Map(dest => dest.ProfilePicture, src => src.ProfilePicture);

            config.NewConfig<Rating, UserRatingDto>();

            config.NewConfig<Post, UserPostDto>()
                .Map(dest => dest.UpvoteCount, src => src.Votes != null ? src.Votes.Count(v => v.IsUpvote) : 0)
                .Map(dest => dest.DownvoteCount, src => src.Votes != null ? src.Votes.Count(v => !v.IsUpvote) : 0)
                .Map(dest => dest.CommentCount, src => src.Comments != null ? src.Comments.Count : 0);
        }
    }
}