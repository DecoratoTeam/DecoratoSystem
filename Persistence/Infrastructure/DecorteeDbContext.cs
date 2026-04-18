using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class DecorteeDbContext(DbContextOptions<DecorteeDbContext> options) 
    : DbContext(options)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global default for SQL Server: avoid multiple cascade paths
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }

        // Explicit cascades that are safe and required by business flow
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SavedPost>()
            .HasOne(sp => sp.Post)
            .WithMany(p => p.SavedPosts)
            .HasForeignKey(sp => sp.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.Post)
            .WithMany(p => p.Votes)
            .HasForeignKey(v => v.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.ShowcaseDesign)
            .WithMany(sd => sd.Ratings)
            .HasForeignKey(r => r.ShowcaseDesignId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SavedDesign>()
            .HasOne(sd => sd.ShowcaseDesign)
            .WithMany(s => s.SavedDesigns)
            .HasForeignKey(sd => sd.ShowcaseDesignId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vote: Unique constraint (one vote per user per post)
        modelBuilder.Entity<Vote>()
            .HasIndex(v => new { v.PostId, v.UserId })
            .IsUnique();

        // Rating: Unique constraint (one rating per user per design)
        modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.ShowcaseDesignId, r.UserId })
            .IsUnique();

        // ChatMessage: Index on ConversationId for fast lookups
        modelBuilder.Entity<ChatMessage>()
            .HasIndex(c => c.ConversationId);

        // ShowcaseDesign relationships with DeleteBehavior.Restrict
        modelBuilder.Entity<ShowcaseDesign>()
            .HasOne(s => s.RoomType)
            .WithMany(r => r.ShowcaseDesigns)
            .HasForeignKey(s => s.RoomTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShowcaseDesign>()
            .HasOne(s => s.DesignStyle)
            .WithMany(d => d.ShowcaseDesigns)
            .HasForeignKey(s => s.DesignStyleId)
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
    }
}