using System.ComponentModel.DataAnnotations;
using src.Models.UserModel;
using src.Models.TodoModel;
using System.Text.Json.Serialization;

namespace src.Models.CommentModel;

public class CommentDto {
    [Required]
    public string? Context { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public int TodoId { get; set; }
}