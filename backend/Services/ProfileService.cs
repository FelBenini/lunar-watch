using lunarwatch.backend.DTO;
using lunarwatch.backend.Models;

namespace lunarwatch.backend.Services;

public class ProfileService {
  public UserConvertToProfileDTO convertToProfileDTO(Profile profile)
  {
    return new UserConvertToProfileDTO(profile);
  }
}