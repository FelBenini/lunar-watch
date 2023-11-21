using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace lunarwatch.backend.Models;

public class Profile {
  [Key, Required, NotNull]
  public int Id { get; set; }
  [Required, NotNull]
  public string? Username { get; set; }
  public string? DisplayName { get; set; }
  [Required, NotNull]
  public int UserId { get; set; }
  public ApplicationUser? User { get; set; }
  public int FollowersCount { get; set; } = 0;
  public int FollowingCount { get; set; } = 0;
  public DateTime? CreatedAt { get; set; } = DateTime.Now;
  public int PostCount { get; set; } = 0;
}