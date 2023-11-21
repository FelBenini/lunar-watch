using lunarwatch.backend.DTO;
using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lunarwatch.backend.Controllers;

[ApiController]
[Route("/api/post")]
public class PostController : ControllerBase
{
  private readonly DatabaseContext _databaseContext;
  public PostController(DatabaseContext databaseContext)
  {
    _databaseContext = databaseContext;
  }

  [HttpGet]
  public IActionResult GetPostByProfileAndTitle([FromQuery] string username, [FromQuery] string title)
  {
    Profile? profile = _databaseContext.Profiles.FirstOrDefault(p => p.Username == username);
    if (profile != null)
    {
      Post? post = _databaseContext.Posts.Where(p => p.ProfileId == profile.Id && p.Title == title).FirstOrDefault();
      if (post != null) return Ok(post);
    }
    return NotFound();
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> CreatePost([FromBody] PostRequestDTO postRequest)
  {
    string username = User.Identity.Name;
    Profile profile = await _databaseContext.Profiles.FirstOrDefaultAsync(p => p.Username == username);
    Post post = new Post(postRequest.Title, postRequest.Content, $"{profile.Username}/{postRequest.Title}", profile.Id);
    await _databaseContext.Posts.AddAsync(post);
    await _databaseContext.SaveChangesAsync();
    return CreatedAtAction(nameof(GetPostByProfileAndTitle), new {title = post.Title, username = profile.Username});
  }
}