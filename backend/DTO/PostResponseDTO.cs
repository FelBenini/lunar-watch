
using lunarwatch.backend.DTO;
using lunarwatch.backend.Models;

namespace lunarwatch.backend.DTO;

public class PostResponse
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }
  public string PostUrl { get; set; }
  public int ReactionCount { get; set; } = 0;
  public UserConvertToProfileDTO Profile { get; set; }
  public DateTime? PublishedAt { get; set; }
  public string? Image { get; set; }
  public bool? IsLiked { get; set; }
  public ReactionType? ReactionType { get; set; }

  public PostResponse(Post post, UserConvertToProfileDTO profile, bool? isLiked = false, ReactionType? reactionType = null)
  {
    Title = post.Title;
    Content = post.Content;
    PostUrl = post.PostUrl;
    ReactionCount = post.ReactionCount;
    Profile = profile;
    PublishedAt = post.PublishedAt;
    Image = post.Image;
    IsLiked = isLiked;
    ReactionType = reactionType;
  }
}