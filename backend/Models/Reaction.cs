using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace lunarwatch.backend.Models;

public class Reaction
{
  [Required, Key, NotNull]
  public int Id { get; set; }
  [Required, NotNull]
  public int PostId { get; set; }
  [Required, NotNull]
  public int ProfileId { get; set; }
  [Required, NotNull]
  public ReactionType ReactionType { get; set; } = ReactionType.Like;
  public Reaction(int postId, int profileId, ReactionType reactionType = ReactionType.Like)
  {
    PostId = postId;
    ProfileId = profileId;
    ReactionType = reactionType;
  }
}

public enum ReactionType
{
  Like = 0,
  Heart = 1,
  Angry = 2,
  Applaud = 3
}