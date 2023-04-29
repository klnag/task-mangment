using System.ComponentModel.DataAnnotations;
using src.Models.UserModel;
using src.Models.TodoModel;
using System.Text.Json.Serialization;

namespace src.Models.CommentModel;

public class Comment {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Context { get; set; }
    public string? UserName { get; set; }
    public int UserId { get; set; }
    public int TodoId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public Todo? Todo { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}