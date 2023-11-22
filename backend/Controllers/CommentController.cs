using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using lunarwatch.backend.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lunarwatch.backend.Controllers;

[ApiController]
[Route("/api/comment")]
public class CommentController: ControllerBase
{
  private readonly DatabaseContext _databaseContext;
  public CommentController(DatabaseContext databaseContext)
  {
    _databaseContext = databaseContext;
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> ReplyToAPost(int postId, [FromBody] CommentRequestDTO body)
  {
    string? username = User.Identity?.Name;
    Profile? profile = await _databaseContext.Profiles.FirstOrDefaultAsync(p => p.Username == username);
    Post? post = await _databaseContext.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.Published == true);
    if (post == null) return NotFound("Post was not found");

    Comment comment = new Comment
    {
      PostId = postId,
      ProfileId = profile.Id,
      Content = body.Content
    };

    _databaseContext.Comments.Add(comment);
    _databaseContext.SaveChanges();

    return Ok(comment);
  }

  [HttpPost("reply-comment")]
  [Authorize]
  public async Task<IActionResult> ReplyToAComment(int commentId, [FromBody] CommentRequestDTO body)
  {
    string? username = User.Identity?.Name;
    Profile? profile = await _databaseContext.Profiles.FirstOrDefaultAsync(p => p.Username == username);
    Comment? mainComment = await _databaseContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
    if (mainComment == null) return NotFound("Comment was not found");

    Comment comment = new Comment
    {
      CommentId = mainComment.Id,
      ProfileId = profile.Id,
      Content = body.Content
    };

    _databaseContext.Comments.Add(comment);
    _databaseContext.SaveChanges();

    return Ok(comment);
  }

  [HttpGet]
  public IActionResult GetAComment(int commentId)
  {
    Comment? comment = _databaseContext.Comments.FirstOrDefault(c => c.Id == commentId);
    if (comment == null) return NotFound();
    return Ok(comment);
  }
}