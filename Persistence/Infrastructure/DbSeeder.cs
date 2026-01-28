using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(DecorteeDbContext context)
        {
            if (await context.RoomTypes.AnyAsync())
                return;

            // Seed RoomTypes
            var roomTypes = new List<RoomType>
            {
                new() { Name = "Living Room", Description = "Main living and entertainment area" },
                new() { Name = "Bedroom", Description = "Sleeping and relaxation space" },
                new() { Name = "Kitchen", Description = "Cooking and meal preparation area" },
                new() { Name = "Bathroom", Description = "Personal hygiene and grooming space" },
                new() { Name = "Office", Description = "Work from home workspace" },
                new() { Name = "Dining Room", Description = "Formal dining and gathering area" }
            };
            await context.RoomTypes.AddRangeAsync(roomTypes);

            // Seed DesignStyles
            var designStyles = new List<DesignStyle>
            {
                new() { Name = "Modern", Description = "Clean lines, minimal ornamentation, and functional design" },
                new() { Name = "Minimalist", Description = "Simple, uncluttered spaces with essential elements only" },
                new() { Name = "Rustic", Description = "Natural materials, warm tones, and countryside charm" },
                new() { Name = "Industrial", Description = "Raw materials, exposed elements, and urban aesthetics" },
                new() { Name = "Scandinavian", Description = "Light colors, natural materials, and cozy simplicity" },
                new() { Name = "Traditional", Description = "Classic elegance, rich colors, and timeless appeal" }
            };
            await context.DesignStyles.AddRangeAsync(designStyles);

            // Seed sample Users (Phone removed)
            var passwordHasher = new PasswordHasher<User>();
            var users = new List<User>
            {
                new()
                {
                    Name = "Demo User",
                    UserName = "demouser",
                    Email = "demo@decorato.com",
                    Role = Role.Customer
                },
                new()
                {
                    Name = "Admin User",
                    UserName = "admin",
                    Email = "admin@decorato.com",
                    Role = Role.Admin
                }
            };

            foreach (var user in users)
            {
                user.Password = passwordHasher.HashPassword(user, "Demo123!");
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Seed ShowcaseDesigns (10+ with mix of Popular/Trending)
            var showcaseDesigns = new List<ShowcaseDesign>
            {
                new()
                {
                    Title = "Modern Living Room Elegance",
                    Description = "A sleek modern living room with clean lines and neutral tones",
                    ImageUrl = "/static/showcase-modern-living.jpg",
                    RoomTypeId = roomTypes[0].Id,
                    DesignStyleId = designStyles[0].Id,
                    UserId = users[0].Id,
                    IsPopular = true,
                    IsTrending = true
                },
                new()
                {
                    Title = "Minimalist Bedroom Retreat",
                    Description = "A peaceful minimalist bedroom with essential furnishings",
                    ImageUrl = "/static/showcase-minimal-bedroom.jpg",
                    RoomTypeId = roomTypes[1].Id,
                    DesignStyleId = designStyles[1].Id,
                    UserId = users[0].Id,
                    IsPopular = true,
                    IsTrending = false
                },
                new()
                {
                    Title = "Rustic Kitchen Charm",
                    Description = "A warm rustic kitchen with natural wood elements",
                    ImageUrl = "/static/showcase-rustic-kitchen.jpg",
                    RoomTypeId = roomTypes[2].Id,
                    DesignStyleId = designStyles[2].Id,
                    UserId = users[0].Id,
                    IsPopular = false,
                    IsTrending = true
                },
                new()
                {
                    Title = "Industrial Loft Office",
                    Description = "An urban industrial home office with exposed brick",
                    ImageUrl = "/static/showcase-industrial-office.jpg",
                    RoomTypeId = roomTypes[4].Id,
                    DesignStyleId = designStyles[3].Id,
                    UserId = users[0].Id,
                    IsPopular = true,
                    IsTrending = true
                },
                new()
                {
                    Title = "Scandinavian Bathroom Spa",
                    Description = "A light and airy Scandinavian bathroom design",
                    ImageUrl = "/static/showcase-scandi-bathroom.jpg",
                    RoomTypeId = roomTypes[3].Id,
                    DesignStyleId = designStyles[4].Id,
                    UserId = users[0].Id,
                    IsPopular = true,
                    IsTrending = false
                },
                new()
                {
                    Title = "Traditional Dining Elegance",
                    Description = "A classic traditional dining room with rich wood tones",
                    ImageUrl = "/static/showcase-traditional-dining.jpg",
                    RoomTypeId = roomTypes[5].Id,
                    DesignStyleId = designStyles[5].Id,
                    UserId = users[0].Id,
                    IsPopular = false,
                    IsTrending = true
                },
                new()
                {
                    Title = "Modern Kitchen Innovation",
                    Description = "A contemporary kitchen with smart storage solutions",
                    ImageUrl = "/static/showcase-modern-kitchen.jpg",
                    RoomTypeId = roomTypes[2].Id,
                    DesignStyleId = designStyles[0].Id,
                    UserId = users[1].Id,
                    IsPopular = true,
                    IsTrending = true
                },
                new()
                {
                    Title = "Minimalist Office Focus",
                    Description = "A distraction-free minimalist workspace",
                    ImageUrl = "/static/showcase-minimal-office.jpg",
                    RoomTypeId = roomTypes[4].Id,
                    DesignStyleId = designStyles[1].Id,
                    UserId = users[1].Id,
                    IsPopular = false,
                    IsTrending = false
                },
                new()
                {
                    Title = "Industrial Living Space",
                    Description = "A bold industrial living room with metal accents",
                    ImageUrl = "/static/showcase-industrial-living.jpg",
                    RoomTypeId = roomTypes[0].Id,
                    DesignStyleId = designStyles[3].Id,
                    UserId = users[1].Id,
                    IsPopular = true,
                    IsTrending = false
                },
                new()
                {
                    Title = "Scandinavian Bedroom Bliss",
                    Description = "A cozy Scandinavian bedroom with natural textures",
                    ImageUrl = "/static/showcase-scandi-bedroom.jpg",
                    RoomTypeId = roomTypes[1].Id,
                    DesignStyleId = designStyles[4].Id,
                    UserId = users[1].Id,
                    IsPopular = false,
                    IsTrending = true
                },
                new()
                {
                    Title = "Rustic Bathroom Retreat",
                    Description = "A charming rustic bathroom with stone elements",
                    ImageUrl = "/static/showcase-rustic-bathroom.jpg",
                    RoomTypeId = roomTypes[3].Id,
                    DesignStyleId = designStyles[2].Id,
                    UserId = users[0].Id,
                    IsPopular = true,
                    IsTrending = true
                }
            };
            await context.ShowcaseDesigns.AddRangeAsync(showcaseDesigns);

            // Seed Posts
            var posts = new List<Post>
            {
                new()
                {
                    Title = "My Living Room Transformation",
                    Content = "Just finished redesigning my living room with a modern aesthetic. The clean lines and neutral palette create such a calming atmosphere!",
                    ImageUrl = "/static/post-living-transform.jpg",
                    UserId = users[0].Id
                },
                new()
                {
                    Title = "Tips for Small Space Design",
                    Content = "Living in a small apartment? Here are my top tips for maximizing space while maintaining style...",
                    ImageUrl = "/static/post-small-space.jpg",
                    UserId = users[0].Id
                },
                new()
                {
                    Title = "Industrial vs Modern: Which is Right for You?",
                    Content = "Comparing two popular design styles and helping you decide which fits your lifestyle better.",
                    ImageUrl = "/static/post-style-compare.jpg",
                    UserId = users[1].Id
                }
            };
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();

            // Seed Comments
            var comments = new List<Comment>
            {
                new()
                {
                    PostId = posts[0].Id,
                    UserId = users[1].Id,
                    Content = "This looks amazing! What paint color did you use?"
                },
                new()
                {
                    PostId = posts[0].Id,
                    UserId = users[0].Id,
                    Content = "Thank you! It's Benjamin Moore Simply White."
                },
                new()
                {
                    PostId = posts[1].Id,
                    UserId = users[1].Id,
                    Content = "Great tips! The mirror trick really works."
                }
            };
            await context.Comments.AddRangeAsync(comments);

            // Seed Ratings
            var ratings = new List<Rating>
            {
                new()
                {
                    ShowcaseDesignId = showcaseDesigns[0].Id,
                    UserId = users[1].Id,
                    Value = 5,
                    Review = "Absolutely stunning design! Love the color palette."
                },
                new()
                {
                    ShowcaseDesignId = showcaseDesigns[1].Id,
                    UserId = users[1].Id,
                    Value = 4,
                    Review = "Very peaceful and calming. Great for relaxation."
                },
                new()
                {
                    ShowcaseDesignId = showcaseDesigns[3].Id,
                    UserId = users[0].Id,
                    Value = 5,
                    Review = "Perfect workspace inspiration!"
                }
            };
            await context.Ratings.AddRangeAsync(ratings);

            // Seed Votes
            var votes = new List<Vote>
            {
                new() { PostId = posts[0].Id, UserId = users[1].Id, IsUpvote = true },
                new() { PostId = posts[1].Id, UserId = users[1].Id, IsUpvote = true },
                new() { PostId = posts[2].Id, UserId = users[0].Id, IsUpvote = true }
            };
            await context.Votes.AddRangeAsync(votes);

            await context.SaveChangesAsync();
        }
    }
}
