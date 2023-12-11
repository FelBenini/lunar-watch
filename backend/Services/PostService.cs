using lunarwatch.backend.DTO;
using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;

namespace lunarwatch.backend.Services;

public class PostService
{
  private readonly DatabaseContext _databaseContext;
  private readonly ProfileService _profileService;
  public PostService(DatabaseContext databaseContext, ProfileService profileService)
  {
    _databaseContext = databaseContext;
    _profileService = profileService;
  }

  public PostResponse ConvertToPostResponse(Post post, string? requestorName)
  {
    if (requestorName == null) return ConvertToPostResponse(post);
    UserConvertToProfileDTO profileDTO = _profileService.ConvertToProfileDTO(post.Profile, requestorName);

    bool isLiked = false;
    var profileReq = _databaseContext.Profiles.Select(p => new { p.Id, p.Username }).Where(p => p.Username == requestorName).FirstOrDefault();
    Reaction? reaction = _databaseContext.Reactions.FirstOrDefault(r => r.PostId == post.Id && r.ProfileId == profileReq.Id);
    if (reaction != null) isLiked = true;

    return new PostResponse(post, profileDTO, isLiked, reaction?.ReactionType);
  }

  public PostResponse ConvertToPostResponse(Post post)
  {
    UserConvertToProfileDTO profileDTO = _profileService.ConvertToProfileDTO(post.Profile);
    return new PostResponse(post, profileDTO);
  }
}