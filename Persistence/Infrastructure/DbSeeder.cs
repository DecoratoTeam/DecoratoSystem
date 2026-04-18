using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(DecorteeDbContext context)
        {
            if (await context.RoomTypes.AnyAsync())
                return;

            // ── 1. RoomTypes ────────────────────────────────────────────────
            // IconUrl: first image per room type (assigned after we know the image list)
            var roomTypes = new List<RoomType>
            {
                new() { Name = "Living Room", Description = "Main living and entertainment area",  IconUrl = "/static/interior-0.jpg"  },
                new() { Name = "Bedroom",     Description = "Sleeping and relaxation space",       IconUrl = "/static/interior-7.jpg"  },
                new() { Name = "Kitchen",     Description = "Cooking and meal preparation area",   IconUrl = "/static/interior-8.jpg"  },
                new() { Name = "Bathroom",    Description = "Personal hygiene and grooming space", IconUrl = "/static/interior-20.jpg" },
                new() { Name = "Office",      Description = "Work from home workspace",            IconUrl = "/static/interior-30.jpg" },
                new() { Name = "Dining Room", Description = "Formal dining and gathering area",    IconUrl = "/static/interior-40.jpg" },
            };
            await context.RoomTypes.AddRangeAsync(roomTypes);

            // ── 2. DesignStyles ─────────────────────────────────────────────
            var designStyles = new List<DesignStyle>
            {
                new() { Name = "Modern",       Description = "Clean lines, minimal ornamentation, and functional design", PreviewImage = "/static/interior-0.jpg"  },
                new() { Name = "Minimalist",   Description = "Simple, uncluttered spaces with essential elements only",   PreviewImage = "/static/interior-1.jpg"  },
                new() { Name = "Rustic",       Description = "Natural materials, warm tones, and countryside charm",      PreviewImage = "/static/interior-2.jpg"  },
                new() { Name = "Industrial",   Description = "Raw materials, exposed elements, and urban aesthetics",     PreviewImage = "/static/interior-3.jpg"  },
                new() { Name = "Scandinavian", Description = "Light colors, natural materials, and cozy simplicity",     PreviewImage = "/static/interior-4.jpg"  },
                new() { Name = "Traditional",  Description = "Classic elegance, rich colors, and timeless appeal",       PreviewImage = "/static/interior-5.jpg"  },
            };
            await context.DesignStyles.AddRangeAsync(designStyles);

            // ── 3. Users ────────────────────────────────────────────────────
            var passwordHasher = new PasswordHasher<User>();
            var users = new List<User>
            {
                new() { Name = "Demo User",  UserName = "demouser", Email = "demo@decorato.com",  Role = Role.Customer },
                new() { Name = "Admin User", UserName = "admin",    Email = "admin@decorato.com", Role = Role.Admin    },
            };
            foreach (var u in users)
                u.Password = passwordHasher.HashPassword(u, "Demo123!");

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // ── 4. ShowcaseDesigns – loaded from SeederData.json ────────────
            var jsonPath = ResolveSeederJson();
            var showcaseDesigns = new List<ShowcaseDesign>();

            if (jsonPath != null)
            {
                var items = JsonSerializer.Deserialize<List<JsonElement>>(
                    await File.ReadAllTextAsync(jsonPath)) ?? new();

                foreach (var item in items)
                {
                    var roomName  = item.GetProperty("room").GetString()  ?? "Living Room";
                    var styleName = item.GetProperty("style").GetString() ?? "Modern";
                    var roomType  = roomTypes.FirstOrDefault(r => r.Name == roomName)    ?? roomTypes[0];
                    var styleType = designStyles.FirstOrDefault(s => s.Name == styleName) ?? designStyles[0];
                    var idx       = item.GetProperty("index").GetInt32();

                    showcaseDesigns.Add(new ShowcaseDesign
                    {
                        Title         = item.GetProperty("description").GetString() ?? $"Design #{idx}",
                        Description   = item.GetProperty("description").GetString() ?? "",
                        ImageUrl      = item.GetProperty("file").GetString()        ?? $"/static/interior-{idx}.jpg",
                        RoomTypeId    = roomType.Id,
                        DesignStyleId = styleType.Id,
                        UserId        = (idx % 2 == 0) ? users[0].Id : users[1].Id,
                        IsPopular     = item.GetProperty("popular").GetBoolean(),
                        IsTrending    = item.GetProperty("trending").GetBoolean(),
                        AverageRating = item.GetProperty("rating").GetDouble(),
                        ViewCount     = item.GetProperty("views").GetInt32(),
                    });
                }

                // back-fill IconUrl / PreviewImage with real images by category
                UpdateRoomTypeIcons(roomTypes, showcaseDesigns);
                UpdateStylePreviews(designStyles, showcaseDesigns);
                context.RoomTypes.UpdateRange(roomTypes);
                context.DesignStyles.UpdateRange(designStyles);
            }
            else
            {
                // Fallback: 30 generic designs when JSON is missing
                var rng  = new Random(42);
                var adj  = new[] { "Elegant","Cozy","Luxury","Chic","Serene","Bold","Timeless","Fresh","Warm","Sleek" };
                var rmCy = new[] { "Living Room","Bedroom","Kitchen","Bathroom","Office","Dining Room" };
                var stCy = new[] { "Modern","Minimalist","Rustic","Industrial","Scandinavian","Traditional" };
                for (int i = 0; i < 30; i++)
                {
                    var rt = roomTypes.FirstOrDefault(r => r.Name == rmCy[i % 6])    ?? roomTypes[i % 6];
                    var st = designStyles.FirstOrDefault(s => s.Name == stCy[i % 6]) ?? designStyles[i % 6];
                    showcaseDesigns.Add(new ShowcaseDesign
                    {
                        Title         = $"{adj[i % adj.Length]} {rmCy[i % 6]} Design",
                        Description   = $"A beautiful {stCy[i % 6].ToLower()} {rmCy[i % 6].ToLower()} design.",
                        ImageUrl      = $"/static/interior-{i}.jpg",
                        RoomTypeId    = rt.Id,
                        DesignStyleId = st.Id,
                        UserId        = (i % 2 == 0) ? users[0].Id : users[1].Id,
                        IsPopular     = i < 8,
                        IsTrending    = i % 4 == 0,
                        AverageRating = Math.Round(rng.NextDouble() * 3 + 7, 1),
                        ViewCount     = rng.Next(200, 8000),
                    });
                }
            }

            await context.ShowcaseDesigns.AddRangeAsync(showcaseDesigns);

            // ── 5. Posts ─────────────────────────────────────────────────────
            var posts = new List<Post>
            {
                new() { Title = "My Living Room Transformation",         Content = "Just finished redesigning my living room with a modern aesthetic. The clean lines and neutral palette create such a calming atmosphere!", ImageUrl = "/static/interior-0.jpg",  UserId = users[0].Id },
                new() { Title = "Tips for Small Space Design",           Content = "Living in a small apartment? Here are my top tips for maximizing space while maintaining style...",                                       ImageUrl = "/static/interior-10.jpg", UserId = users[0].Id },
                new() { Title = "Industrial vs Modern: Which is Right?", Content = "Comparing two popular design styles and helping you decide which fits your lifestyle better.",                                           ImageUrl = "/static/interior-20.jpg", UserId = users[1].Id },
            };
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();

            // ── 6. Comments ───────────────────────────────────────────────────
            await context.Comments.AddRangeAsync(new List<Comment>
            {
                new() { PostId = posts[0].Id, UserId = users[1].Id, Content = "This looks amazing! What paint color did you use?" },
                new() { PostId = posts[0].Id, UserId = users[0].Id, Content = "Thank you! It's Benjamin Moore Simply White." },
                new() { PostId = posts[1].Id, UserId = users[1].Id, Content = "Great tips! The mirror trick really works." },
            });

            // ── 7. Ratings ────────────────────────────────────────────────────
            if (showcaseDesigns.Count >= 4)
            {
                await context.Ratings.AddRangeAsync(new List<Rating>
                {
                    new() { ShowcaseDesignId = showcaseDesigns[0].Id, UserId = users[1].Id, Value = 5, Review = "Absolutely stunning design! Love the color palette." },
                    new() { ShowcaseDesignId = showcaseDesigns[1].Id, UserId = users[1].Id, Value = 4, Review = "Very peaceful and calming. Great for relaxation."    },
                    new() { ShowcaseDesignId = showcaseDesigns[3].Id, UserId = users[0].Id, Value = 5, Review = "Perfect workspace inspiration!"                       },
                });
            }

            // ── 8. Votes ──────────────────────────────────────────────────────
            await context.Votes.AddRangeAsync(new List<Vote>
            {
                new() { PostId = posts[0].Id, UserId = users[1].Id, IsUpvote = true },
                new() { PostId = posts[1].Id, UserId = users[1].Id, IsUpvote = true },
                new() { PostId = posts[2].Id, UserId = users[0].Id, IsUpvote = true },
            });

            await context.SaveChangesAsync();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// Finds SeederData.json next to the exe or 4 levels up (dev mode).
        private static string? ResolveSeederJson()
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "SeederData.json"),
                Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                    "../../../../Persistence/Infrastructure/SeederData.json")),
            };
            return candidates.FirstOrDefault(File.Exists);
        }

        /// Assigns the first image found per room to that room's IconUrl.
        private static void UpdateRoomTypeIcons(List<RoomType> roomTypes, List<ShowcaseDesign> designs)
        {
            foreach (var rt in roomTypes)
            {
                var first = designs.FirstOrDefault(d => d.RoomTypeId == rt.Id);
                if (first != null) rt.IconUrl = first.ImageUrl;
            }
        }

        /// Assigns the first image found per style to that style's PreviewImage.
        private static void UpdateStylePreviews(List<DesignStyle> styles, List<ShowcaseDesign> designs)
        {
            foreach (var st in styles)
            {
                var first = designs.FirstOrDefault(d => d.DesignStyleId == st.Id);
                if (first != null) st.PreviewImage = first.ImageUrl;
            }
        }
    }
}
