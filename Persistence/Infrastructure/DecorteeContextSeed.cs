
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecorteeSystem.Infrastructure.Persistence
{
    public static class DecorteeContextSeed
    {
        public static async Task SeedAsync(DecorteeDbContext context)
        {
           
            //if (!await context.RoomTypes.AnyAsync())
            //{
            //    var types = new List<RoomType>
            //    {
            //        new RoomType { Name = "Living Room", IconUrl = "https://cdn-icons-png.flaticon.com/512/2361/2361093.png" },
            //        new RoomType { Name = "Kitchen", IconUrl = "https://cdn-icons-png.flaticon.com/512/1027/1027352.png" },
            //        new RoomType { Name = "Bedroom", IconUrl = "https://cdn-icons-png.flaticon.com/512/3030/3030336.png" },
            //        new RoomType { Name = "Bathroom", IconUrl = "https://cdn-icons-png.flaticon.com/512/934/934822.png" },
            //        new RoomType { Name = "Dining Room", IconUrl = "https://cdn-icons-png.flaticon.com/512/1160/1160431.png" },
            //        new RoomType { Name = "Office", IconUrl = "https://cdn-icons-png.flaticon.com/512/1086/1086434.png" }
            //    };
            //    await context.RoomTypes.AddRangeAsync(types);
            //    await context.SaveChangesAsync();
            //}

           
            //if (!await context.DesignStyles.AnyAsync())
            //{
            //    var styles = new List<DesignStyle>
            //    {
            //        new DesignStyle { Name = "Modern", PreviewImage = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400" },
            //        new DesignStyle { Name = "Bohemian", PreviewImage = "https://images.unsplash.com/photo-1550226891-ef816aed4a98?w=400" },
            //        new DesignStyle { Name = "Classic", PreviewImage = "https://images.unsplash.com/photo-1505691938895-1758d7eaa511?w=400" },
            //        new DesignStyle { Name = "Minimalist", PreviewImage = "https://images.unsplash.com/photo-1494438639946-1ebd1d20bf85?w=400" }
            //    };
            //    await context.DesignStyles.AddRangeAsync(styles);
            //    await context.SaveChangesAsync();
            //}

            
            //if (!await context.ShowcaseDesigns.AnyAsync())
            //{
            //    var roomIds = await context.RoomTypes.Select(r => r.Id).ToListAsync();
            //    var styleIds = await context.DesignStyles.Select(s => s.Id).ToListAsync();
            //    var random = new Random();

            //    var cities = new[] { "Cairo", "Dubai", "New York", "London", "Paris", "Riyadh", "Tokyo" };
            //    var adjectives = new[] { "Luxury", "Elegant", "Cozy", "Modern", "Royal", "Urban", "Spacious" };

            //    var designs = new List<ShowcaseDesign>();

            //    for (int i = 1; i <= 500; i++)
            //    {
            //        var roomId = roomIds[random.Next(roomIds.Count)];
            //        var styleId = styleIds[random.Next(styleIds.Count)];

            //        // هنجيب اسم الغرفة عشان نطلب صورة شبهها من النت
            //        var roomName = (await context.RoomTypes.FindAsync(roomId)).Name;
            //        var city = cities[random.Next(cities.Length)];
            //        var adj = adjectives[random.Next(adjectives.Length)];

            //        designs.Add(new ShowcaseDesign
            //        {
            //            Title = $"{adj} {roomName} Design",
            //            Location = $"{city}, {adj} District",
            //            // الرابط ده بيولد صورة ديكور حقيقية ومختلفة لكل ID
            //            ImageUrl = $"https://picsum.photos/seed/{i + 100}/800/600",
            //            RoomTypeId = roomId,
            //            DesignStyleId = styleId,
            //            IsPopular = i <= 20, // أول 20 صورة هيبقوا Popular
            //            IsTrending = i % 10 == 0,
            //            AverageRating = Math.Round(random.NextDouble() * (10 - 5) + 5, 1),
            //            ViewCount = random.Next(100, 10000)
            //        });
            //    }

            //    await context.ShowcaseDesigns.AddRangeAsync(designs);
            //    await context.SaveChangesAsync();
            //}
        }
    }
}