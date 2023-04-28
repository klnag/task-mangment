using System.ComponentModel.DataAnnotations;
using src.Models.ProjectModel;

namespace src.Models.UserModel;

public class UserDto {
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}