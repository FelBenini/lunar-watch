using lunarwatch.backend.DTO;
using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;

namespace lunarwatch.backend.Services;

public class ProfileService
{
  private readonly DatabaseContext _databaseContext;
  public ProfileService(DatabaseContext databaseContext)
  {
    _databaseContext = databaseContext;
  }
  public UserConvertToProfileDTO ConvertToProfileDTO(Profile profile)
  {
    return new UserConvertToProfileDTO(profile);
  }

  public UserConvertToProfileDTO ConvertToProfileDTO(Profile profile, string? requestorName)
  {
    if (requestorName == null) return ConvertToProfileDTO(profile);
    Profile? profileReq = _databaseContext.Profiles.FirstOrDefault(p => p.Username == requestorName);
    
    int IsFollowingMe = _databaseContext.Followers.Count(p => p.FollowerProfileId == profile.Id && p.ProfileBeingFollowedId == profileReq.Id);
    int AmFollower = _databaseContext.Followers.Count(p => p.FollowerProfileId == profileReq.Id && p.ProfileBeingFollowedId == profile.Id);
    return new UserConvertToProfileDTO(profile, Convert.ToBoolean(IsFollowingMe), Convert.ToBoolean(AmFollower));
  }
}