using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using lunarwatch.backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
  [Authorize]
  public async Task<IActionResult> MyProfile()
  {
    string? username = User.Identity?.Name;
    var user = await _userManager.FindByNameAsync(username);
    _databaseContext.Entry(user).Reference(u => u.Profile).Load();
    return Ok(_profileSerivce.ConvertToProfileDTO(user.Profile));
  }

  [HttpGet]
  public async Task<IActionResult> getByUsername([FromQuery] string? username)
  {
    if (username == null) return BadRequest();
    Profile profile = await _databaseContext.Profiles.FirstOrDefaultAsync(p => p.Username == username);
    if (profile != null) return Ok(_profileSerivce.ConvertToProfileDTO(profile));
    return NotFound();
  }
}