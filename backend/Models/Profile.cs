using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace coffeebeans.backend.Models;

public class Profile {
  [Key, Required, NotNull]
  public int Id { get; set; }
  [Required, NotNull]
  public string Username { get; set; }
  [Required, NotNull]
  public int UserId { get; set; }
  public ApplicationUser? User { get; set; }
}