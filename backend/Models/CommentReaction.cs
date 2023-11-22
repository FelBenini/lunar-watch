using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace lunarwatch.backend.Models;

public class CommentReaction
{
  [Key, Required, NotNull]
  public int Id { get; set; }
  public int CommentId { get; set; }
  public int ProfileId { get; set; }
  public ReactionType ReactionType { get; set; } = 0;
}