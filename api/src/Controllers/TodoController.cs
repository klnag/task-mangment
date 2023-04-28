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

    [HttpGet()]
    public DbSet<Todo>? Get() {
        return context.Todos;
    }

    [HttpPost]
    public ActionResult<Todo> Post([FromBody] TodoDto todoFormBody)
    {
        Project project = context.Projects.FirstOrDefault(u => u.Id == todoFormBody.ProjectId);
        if (project != null)
        {
            
                Todo newTodo = new Todo { Title = todoFormBody.Title, Project = project, username = todoFormBody.username, index = todoFormBody.index, Priority = todoFormBody.Priority, AssignTo = todoFormBody.AssignTo };
                context.Todos.Add(newTodo);
                context.SaveChanges();
                return newTodo;
        }
        return new BadRequestObjectResult("project not found");
    }

    [HttpPatch("{id}")]
    public  ActionResult<Todo> UpdateTaskPosition(int id, [FromBody] TodoDto request)
    {
        Todo taskItem =  context.Todos.Find(id);
        if (taskItem == null)
        {
            return NotFound();
        }

        taskItem.Title = request.Title;
        taskItem.Context = request.Context;
        taskItem.index = request.index;
        taskItem.Status = request.Status;
        taskItem.Priority = request.Priority;
        taskItem.AssignTo = request.AssignTo;
        context.Todos.Update(taskItem);
        context.SaveChanges();

        return taskItem;
    }

    [HttpDelete("{id}")]
    public string Delete(int id)
    {
        Todo? Todo = context.Todos?.Find(id);
        if (Todo == null)
        {
            return "Todo does not exisit";
        }
        context.Todos?.Remove(Todo);
        context.SaveChanges();
        return "Todo deleted";
    }
}