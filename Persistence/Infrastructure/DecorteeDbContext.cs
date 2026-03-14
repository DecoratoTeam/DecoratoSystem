using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DecorteeDbContext(DbContextOptions<DecorteeDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<DesignStyle> DesignStyles { get; set; }
        public DbSet<ShowcaseDesign> ShowcaseDesigns { get; set; }
        public DbSet<AIDesign> AIDesigns { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<SavedDesign> SavedDesigns { get; set; }
        public DbSet<SavedPost> SavedPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vote: Unique constraint (one vote per user per post)
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.PostId, v.UserId })
                .IsUnique();

            // SavedPost: Unique constraint (one save per user per post)
            modelBuilder.Entity<SavedPost>()
                .HasIndex(sp => new { sp.PostId, sp.UserId })
                .IsUnique();

            // Rating: Unique constraint (one rating per user per design)
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.ShowcaseDesignId, r.UserId })
                .IsUnique();

            // ChatMessage: Index on ConversationId for fast lookups
            modelBuilder.Entity<ChatMessage>()
                .HasIndex(c => c.ConversationId);

            // User relationships - prevent cascade delete
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.MyPosts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShowcaseDesign>()
                .HasOne(s => s.DesignStyle)
                .WithMany(d => d.ShowcaseDesigns)
                .HasForeignKey(s => s.DesignStyleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShowcaseDesign>()
                .HasOne(s => s.User)
                .WithMany(u => u.MyShowcaseDesigns)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // AIDesign relationships
            modelBuilder.Entity<AIDesign>()
                .HasOne(a => a.RoomType)
                .WithMany(r => r.AIDesigns)
                .HasForeignKey(a => a.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AIDesign>()
                .HasOne(a => a.DesignStyle)
                .WithMany(d => d.AIDesigns)
                .HasForeignKey(a => a.DesignStyleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AIDesign>()
                .HasOne(a => a.User)
                .WithMany(u => u.MyAIDesigns)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
