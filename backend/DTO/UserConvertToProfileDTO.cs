using lunarwatch.backend.Models;

namespace lunarwatch.backend.DTO;

public class UserConvertToProfileDTO
{
  public int Id { get; set; }
  public string Username { get; set; }
  public UserConvertToProfileDTO(Profile profile)
  {
    Id = profile.Id;
    Username = profile.Username;
  }
}