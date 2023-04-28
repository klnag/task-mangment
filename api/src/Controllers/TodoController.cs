using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.TodoModel;
using src.Models.ProjectModel;
using src.Models.CommentModel;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class TodoController : ControllerBase {
    private readonly DataContext context;

    public TodoController(DataContext context) {
        this.context = context;
    }
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Error { get; set; } = "";
    }
    // [HttpGet()]
    // public DbSet<Todo>? Get() {
    //     return context.Todos;
    // }

    [HttpPost]
    public ActionResult<ApiResponse<Todo>> Post([FromBody] TodoDto todoFormBody)
    {
        ApiResponse<Todo> response = new();
        Project project = context.Projects.FirstOrDefault(u => u.Id == todoFormBody.ProjectId);
        if (project == null)
        {
            response.Error = "project not found";
            return BadRequest(response);
        }
        Todo newTodo = new Todo { Title = todoFormBody.Title, Project = project, username = todoFormBody.username, index = todoFormBody.index, Priority = todoFormBody.Priority, AssignTo = todoFormBody.AssignTo };
        context.Todos.Add(newTodo);
        context.SaveChanges();
        response.Data = newTodo;
        return response;
    }

    [HttpPatch("{id}")]
    public  ActionResult<ApiResponse<Todo>> UpdateTaskPosition(int id, [FromBody] TodoDto request)
    {
        ApiResponse<Todo> response = new();
        Todo taskItem =  context.Todos.Find(id);
        if (taskItem == null)
        {
            response.Error = "Todo does not exist";
            return BadRequest(response);
        }

        taskItem.Title = request.Title;
        taskItem.Context = request.Context;
        taskItem.index = request.index;
        taskItem.Status = request.Status;
        taskItem.Priority = request.Priority;
        taskItem.AssignTo = request.AssignTo;
        context.Todos.Update(taskItem);
        context.SaveChanges();

        response.Data = taskItem;
        return response;
    }

    [HttpDelete("{id}")]
    public ActionResult<ApiResponse<string>> Delete(int id)
    {
        ApiResponse<string> response = new();
        Todo? Todo = context.Todos?.Find(id);
        if (Todo == null)
        {
            response.Error = "Todo does not exisit";
            return BadRequest(response);
        }
        context.Todos?.Remove(Todo);
        context.SaveChanges();
        response.Data = "Todo deleted";
        return response;
    }
}