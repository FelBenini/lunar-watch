using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lunarwatch.backend.Infra;

public class DatabaseContext: IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
  }

  public DbSet<Post> Posts { get; set; }
  public DbSet<Profile> Profiles { get; set; }
  public DbSet<Reaction> Reactions { get; set; }
}