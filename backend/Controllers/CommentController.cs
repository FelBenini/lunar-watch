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
    Comment? mainComment = _databaseContext.Comments.Where(c => c.Id == commentId).Include("Comments").FirstOrDefault();
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
    Comment? comment = _databaseContext.Comments.Include(c => c.Profile).FirstOrDefault(c => c.Id == commentId);
    PopulateComment(comment);
    if (comment == null) return NotFound();
    return Ok(comment);
  }

  [HttpGet("post")]
  public async Task<IActionResult> GetCommentsFromPost(int postId)
  {
    var post = await _databaseContext.Posts.Where(p => p.Id == postId && p.Published == true).Select(p => new { p.Id }).FirstOrDefaultAsync();
    if (post == null) return NotFound("Post was not found");

    List<Comment> comments = _databaseContext.Comments.Where(c => c.PostId == postId).ToList();
    foreach (Comment comm in comments)
    {
      PopulateComment(comm);
    }
    return Ok(comments);
  }

  private Comment PopulateComment(Comment comment)
  {
    Comment finalComment = comment;
    List<Comment> subComments = _databaseContext.Comments.Where(c => c.CommentId == comment.Id).ToList();
    finalComment.Comments = subComments;
    if (subComments.Count > 0)
    {
      foreach (Comment comm in subComments)
      {
        PopulateComment(comm);
      }
    }
    return finalComment;
  }
}