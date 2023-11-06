using Microsoft.AspNetCore.Identity;

namespace coffeebeans.backend.Models;

public class ApplicationUser : IdentityUser<int>
{
  public Profile Profile { get; set; }
}