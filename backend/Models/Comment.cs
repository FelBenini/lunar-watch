using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace lunarwatch.backend.Models;

public class Comment
{
  [Required, Key, NotNull]
  public int Id { get; set; }
  [Required, NotNull]
  public string Content { get; set; }
  public int? ReactionCount { get; set; } = 0;
  public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
  public int? CommentId { get; set; } 
  public int? PostId { get; set; }
  public int ProfileId { get; set; }
  public Profile Profile { get; set; }
}