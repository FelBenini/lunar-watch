using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace coffeebeans.backend.Models;

public class Profile {
  [Key]
  [ForeignKey("User")]
  public int UserId;
  [Required, NotNull]
  public string Username;
  public ApplicationUser User { get; set; }
}