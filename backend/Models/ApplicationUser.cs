using Microsoft.AspNetCore.Identity;

namespace lunarwatch.backend.Models;

public class ApplicationUser : IdentityUser<int>
{
  public Profile Profile { get; set; }
}