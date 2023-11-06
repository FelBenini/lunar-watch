using System.ComponentModel.DataAnnotations;

namespace coffeebeans.backend.Models;

class LoginModel
{
  [Required(ErrorMessage = "Username is required")]
  public string? Username { get; set; }
  [Required(ErrorMessage = "E-mail is required")]
  public string? Email { get; set; }
  [Required(ErrorMessage = "Password is required")]
  public string? Password { get; set; }

}