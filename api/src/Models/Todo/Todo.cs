using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using src.Models.ProjectModel;
namespace src.Models.TodoModel;

public class Todo {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string Context { get; set; } = "";
    public string? username { get; set; }
    public float index { get; set; }
    public int ProjectId { get; set; }
    public string Status { get; set; } = "TODO";
    public string? Priority { get; set; }
    [JsonIgnore]
    public Project? Project { get; set; }
    public string? AssignTo { get; set; }
     public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}