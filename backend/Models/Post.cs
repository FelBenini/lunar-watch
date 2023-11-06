using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace coffeebeans.backend.Models;

public class Post
{
  [Required, Key, NotNull]
  public int Id { get; set; }
  [Required, NotNull]
  public string Title { get; set; }
  [Required, NotNull]
  public string Content { get; set; }
  [Required, NotNull]
  public string PostUrl { get; set; }
  public int LikeCount { get; set; }
  [Required, NotNull]
  public int ProfileId { get; set; }

  public Post(int id, string title, string content, string postUrl, int profileId)
  {
    Id = id;
    Title = title;
    Content = content;
    PostUrl = postUrl;
    LikeCount = 0;
    ProfileId = profileId;
  }
}