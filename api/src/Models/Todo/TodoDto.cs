using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using src.Models.ProjectModel;
namespace src.Models.TodoModel;

public class TodoDto {
    public string? Title { get; set; }
    public string Context { get; set; }
    public string username { get; set; }
    public float index { get; set; }
    public int ProjectId { get; set; }
    public string Priority { get; set; }
    public string AssignTo { get; set; }
    public string Status { get; set; } = "TODO";
}