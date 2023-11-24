using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lunarwatch.backend.Infra;

public class DatabaseContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.Entity<Comment>()
    .HasMany(c => c.Comments)
    .WithOne()
    .HasForeignKey(c => c.CommentId);
  }

  public DbSet<Post> Posts { get; set; }
  public DbSet<Profile> Profiles { get; set; }
  public DbSet<Reaction> Reactions { get; set; }
  public DbSet<Comment> Comments { get; set; }
  public DbSet<CommentReaction> CommentReactions { get; set; }
  public DbSet<Follower> Followers { get; set; }
}