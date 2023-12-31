using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lunarwatch.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace lunarwatch.backend.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly RoleManager<IdentityRole<int>> _roleManager;

  public AuthController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<int>> roleManager)
  {
    _userManager = userManager;
    _roleManager = roleManager;
  }

  [HttpPost]
  [Route("login")]
  public async Task<IActionResult> Login([FromBody] LoginModel model)
  {
    var user = await _userManager.FindByNameAsync(model.Username);
    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
    {
      var userRoles = await _userManager.GetRolesAsync(user);

      var authClaims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Email, user.Email)
      };

      foreach (var userRole in userRoles)
      {
        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
      }

      var token = GetToken(authClaims);

      return Ok(new
      {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        expiration = token.ValidTo
      });
    }
    return Unauthorized();
  }

  [HttpPost]
  [Route("register")]
  public async Task<IActionResult> Register([FromBody] RegisterModel model)
  {
    if (model.Username == null || model.Password == null) {
      return BadRequest();
    }
    var userExists = await _userManager.FindByNameAsync(model.Username);
    if (userExists != null)
    {
      return Conflict("User already exists");
    }
    ApplicationUser user = new()
    {
      Email = model.Email,
      UserName = model.Username,
      SecurityStamp = Guid.NewGuid().ToString(),
      Profile = new()
      {
        Username = model.Username
      }
    };
    var result = await _userManager.CreateAsync(user, model.Password);
    if (!result.Succeeded)
    {
      return BadRequest(result);
    }

    var roleExists = await _roleManager.RoleExistsAsync(UserRoles.User);

    if (!roleExists) {
      await CreateRoles();
    }

    await _userManager.AddToRoleAsync(user, UserRoles.User);

    return Ok(result);
  }

  private JwtSecurityToken GetToken(List<Claim> authClaims)
  {
    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "another_secret_keyifnotset"));

    var token = new JwtSecurityToken(
      expires: DateTime.Now.AddDays(7),
      claims: authClaims,
      signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
    );

    return token;
  }

  [HttpGet("session")]
  [Authorize]
  public async Task<IActionResult> GetSession()
  {
    string? token = Request.Headers["Authorization"];
    token = token.Replace("Bearer ", "");
    var session = new JwtSecurityTokenHandler().ReadJwtToken(token);
    return Ok(session);
  }

  private async Task CreateRoles()
  {
    // create the roles for users
    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
      var roleExist = await _roleManager.RoleExistsAsync(roleName);

      if (!roleExist)
      {
        IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
      }
    }
  }
}