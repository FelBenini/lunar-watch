using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lunarwatch.backend.Controllers;

[ApiController]
[Route("api/follow")]
public class FollowController : ControllerBase
{
  private readonly DatabaseContext _databaseContext;
  public FollowController(DatabaseContext databaseContext)
  {
    _databaseContext = databaseContext;
  }

  [HttpGet("profile-followers")]
  public IActionResult GetFollowersFromAProfile(string username, int page = 1)
  {
    int pageNum = page - 1;
    var profile = _databaseContext.Profiles.Select(p => new { p.Username }).FirstOrDefault(p => p.Username == username);
    if (profile == null) return NotFound("Profile does not exist");

    List<Profile> followers = _databaseContext.Followers.Select(p => p.FollowerProfile).Take(25).Skip(pageNum * 25).ToList();

    return Ok(followers);
  }

  [HttpPost("toggle-follow")]
  [Authorize]
  public async Task<IActionResult> ToggleFollow(string username)
  {
    var profileFollowing = await _databaseContext.Profiles.Select(p => new { p.Username, p.Id }).FirstOrDefaultAsync(p => p.Username == User.Identity.Name);
    var profile = await _databaseContext.Profiles.Select(p => new { p.Username, p.Id }).FirstOrDefaultAsync(p => p.Username == username);
    if (profile == null) return NotFound("Profile does not exist");
    Follower? followExists = await _databaseContext.Followers.FirstOrDefaultAsync(f => f.FollowerProfileId == profileFollowing.Id && f.ProfileBeingFollowedId == profile.Id);
    Follower follower = new()
    {
      ProfileBeingFollowedId = profile.Id,
      FollowerProfileId = profileFollowing.Id
    };

    if (followExists == null)
    {
      await _databaseContext.Database.ExecuteSqlRawAsync("UPDATE Profiles SET FollowersCount = FollowersCount + 1 WHERE Id = {0}", profile.Id);
      await _databaseContext.Database.ExecuteSqlRawAsync("UPDATE Profiles SET FollowingCount = FollowingCount + 1 WHERE Id = {0}", profileFollowing.Id);
      _databaseContext.Followers.Add(follower);
      await _databaseContext.SaveChangesAsync();
      return Ok("Follow added");
    }

    await _databaseContext.Database.ExecuteSqlRawAsync("UPDATE Profiles SET FollowersCount = FollowersCount - 1 WHERE Id = {0}", profile.Id);
    await _databaseContext.Database.ExecuteSqlRawAsync("UPDATE Profiles SET FollowingCount = FollowingCount - 1 WHERE Id = {0}", profileFollowing.Id);
    _databaseContext.Followers.Remove(followExists);
    await _databaseContext.SaveChangesAsync();
    return Ok("Follow removed");
  }
}