using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace coffeebeans.backend.Infra;

public class DatabaseContext: IdentityDbContext<IdentityUser>
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
  }
}