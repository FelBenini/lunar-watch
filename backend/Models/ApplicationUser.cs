using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace lunarwatch.backend.Models;

public class ApplicationUser : IdentityUser<int>
{
  [JsonIgnore]
  public Profile Profile { get; set; }
}