using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace lunarwatch.backend.Models;

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
  public int ReactionCount { get; set; } = 0;
  [Required, NotNull]
  public int ProfileId { get; set; }
  public bool? Published { get; set; } = false;
  public DateTime? PublishedAt { get; set; }
  public string? Image { get; set; }

  public Post(string title, string content, string postUrl, int profileId)
  {
    Title = title;
    Content = content;
    PostUrl = postUrl;
    ProfileId = profileId;
  }
}