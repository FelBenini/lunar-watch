using lunarwatch.backend.DTO;
using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using lunarwatch.backend.Services;

namespace lunarwatch.backend.Controllers;

[ApiController]
[Route("/api/profile")]
public class ProfileController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly DatabaseContext _databaseContext;
  private readonly ProfileService _profileSerivce;

  public ProfileController(UserManager<ApplicationUser> userManager, DatabaseContext databaseContext, ProfileService profileService)
  {
    _userManager = userManager;
    _databaseContext = databaseContext;
    _profileSerivce = profileService;
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
    return Ok(_profileSerivce.convertToProfileDTO(user.Profile));
  }

  [HttpGet]
  public async Task<IActionResult> getByUsername([FromQuery] string? username)
  {
    if (username == null) return BadRequest();
    Profile profile = _databaseContext.Profiles.FirstOrDefault(p => p.Username == username);
    if (profile != null) return Ok(_profileSerivce.convertToProfileDTO(profile));
    return NotFound();
  }
}