using lunarwatch.backend.DTO;
using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace lunarwatch.backend.Controllers;

[ApiController]
[Route("/api/profile")]
public class ProfileController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly DatabaseContext _databaseContext;

  public ProfileController(UserManager<ApplicationUser> userManager, DatabaseContext databaseContext)
  {
    _userManager = userManager;
    _databaseContext = databaseContext;
  }

  [HttpGet("me")]
  public async Task<IActionResult> MyProfile()
  {
    string username = User.Identity.Name;
    if (username == null)
    {
      return NotFound();
    }

    var user = await _userManager.FindByNameAsync(username);
    _databaseContext.Entry(user).Reference(u => u.Profile).Load();
    return Ok(new UserConvertToProfileDTO(user.Profile));
  }

  [HttpGet("all")]
  public async Task<IActionResult> allProfiles()
  {
    var users = _databaseContext.Profiles.Select(p => new UserConvertToProfileDTO(p));
    return Ok(users);
  }
}