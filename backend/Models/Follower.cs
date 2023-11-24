using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace lunarwatch.backend.Models;

public class Follower
{
  [Key, Required, NotNull]
  public int Id { get; set; }
  public int ProfileBeingFollowedId { get; set; }
  public Profile? ProfileBeingFollowed { get; set; }
  public int FollowerProfileId { get; set; }
  public Profile? FollowerProfile { get; set; }
}