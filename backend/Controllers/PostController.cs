using lunarwatch.backend.DTO;
using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
      Post? post = _databaseContext.Posts.Where(p => p.ProfileId == profile.Id && p.Title == title && p.Published == true).FirstOrDefault();
      if (post != null) return Ok(post);
    }
    return NotFound();
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> CreatePost([FromBody] PostRequestDTO postRequest)
  {
    string? username = User.Identity?.Name;
    Profile? profile = await _databaseContext.Profiles.FirstOrDefaultAsync(p => p.Username == username);
    Post post = new Post(postRequest.Title, postRequest.Content, $"{profile?.Username}/{postRequest.Title}", profile.Id);
    await _databaseContext.Posts.AddAsync(post);
    await _databaseContext.SaveChangesAsync();
    return CreatedAtAction(nameof(GetPostByProfileAndTitle), new {title = post.Title, username = profile.Username});
  }

  [HttpPut("publish")]
  [Authorize]
  public async Task<IActionResult> PublishPost(int id)
  {
    string? username = User.Identity?.Name;
    Profile? profile = await _databaseContext.Profiles.FirstOrDefaultAsync(p => p.Username == username);
    Post? post = await _databaseContext.Posts.Where(p => p.Id == id).FirstOrDefaultAsync();
    if (post == null) return NotFound();
    if (post.ProfileId != profile.Id) return Unauthorized();
    if (post.Content == "" || post.Title == "") return BadRequest("Title or Content are empty");

    Post? checkForConflictsPost = await _databaseContext.Posts.Where(p => p.Title == post.Title && p.ProfileId == post.ProfileId && p.Published == true).FirstOrDefaultAsync();

    if (checkForConflictsPost != null) return Conflict();

    post.Published = true;
    post.PublishedAt = DateTime.Now;

    _databaseContext.SaveChanges();
    return CreatedAtAction(nameof(GetPostByProfileAndTitle), new {title = post.Title, username = profile.Username});
  }

  [HttpGet("profile")]
  public async Task<IActionResult> GetPostsFromProfile(string username, [FromQuery] int page = 1)
  {
    int pageNum = page - 1;
    var profile = await _databaseContext.Profiles.Where(p => p.Username == username).Select(p => new {p.Username, p.Id}).FirstOrDefaultAsync();
    IEnumerable<Post> posts = _databaseContext.Posts.Where(p => p.ProfileId == profile.Id && p.Published == true).Skip(pageNum * 15).Take(15);
    return Ok(posts);
  }

  [HttpGet("reactions")]
  public async Task<IActionResult> GetReactionsFromPost(int postId, [FromQuery] int page = 1)
  {
    int pageNum = page - 1;
    IEnumerable<Reaction> reactions = _databaseContext.Reactions.Where(r => r.PostId == postId).Skip(pageNum * 30).Take(30);
    return Ok(reactions);
  }

  [HttpPost("react")]
  [Authorize]
  public async Task<IActionResult> ReactToAPost(int postId, [FromBody] ReactionRequestDTO body)
  {
    string? username = User.Identity?.Name;
    Profile? profile = await _databaseContext.Profiles.FirstOrDefaultAsync(p => p.Username == username);
    var post = await _databaseContext.Posts.Where(p => p.Id == postId && p.Published == true).Select(p => new { p.Id }).FirstOrDefaultAsync();
    if (post == null) return NotFound();
    var reactionCheck = await _databaseContext.Reactions.Where(r => r.PostId == postId && r.ProfileId == profile.Id).FirstOrDefaultAsync();
    if (reactionCheck != null)
    {
      _databaseContext.Reactions.Remove(reactionCheck);
      _databaseContext.SaveChanges();
      return Ok("reaction removed");
    }

    Reaction reaction = new Reaction(postId, profile.Id, body.ReactionType);
    await _databaseContext.Reactions.AddAsync(reaction);
    _databaseContext.SaveChanges();
    return Ok("reaction inserted");
  }
}