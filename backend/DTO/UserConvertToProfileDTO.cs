using lunarwatch.backend.Models;

namespace lunarwatch.backend.DTO;

public class UserConvertToProfileDTO
{
  public int Id { get; set; }
  public string Username { get; set; }
  public string DisplayName { get; set; }
  public int FollowersCount { get; set; }
  public int FollowingCount { get; set; }
  public DateTime? CreatedAt { get; set; }
  public string? ProfilePicture { get; set; }
  public UserConvertToProfileDTO(Profile profile)
  {
    Id = profile.Id;
    Username = profile.Username;
    DisplayName = profile.DisplayName ?? profile.Username;
    FollowersCount = profile.FollowersCount;
    FollowingCount = profile.FollowingCount;
    CreatedAt = profile.CreatedAt;
    ProfilePicture = profile.ProfilePicture;
  }
}