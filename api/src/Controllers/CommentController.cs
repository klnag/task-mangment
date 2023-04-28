using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.CommentModel;
using src.Models.TodoModel;
using src.Models.ProjectModel;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class CommentController : ControllerBase {
    private readonly DataContext context;

    public CommentController(DataContext context) {
        this.context = context;
    }

    // [HttpGet()]
    // public DbSet<Comment>? Get() {
    //     return context.Comments;
    // }
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Error { get; set; } = "";
    }

    [HttpPost]
    public ActionResult<ApiResponse<Comment>> Post([FromBody] CommentDto CommentFormBody)
    {
        ApiResponse<Comment> response = new();
        Todo todo = context.Todos.FirstOrDefault(u => u.Id == CommentFormBody.TodoId);
        if (todo == null)
        {
            response.Error = "todo not found";
            return BadRequest(response);
        }
        Comment newComment = new Comment { Context = CommentFormBody.Context, UserId = int.Parse(User.Identity.Name), TodoId = CommentFormBody.TodoId, UserName = CommentFormBody.UserName };
        context.Comments.Add(newComment);
        context.SaveChanges();
        response.Data = newComment;
        return response;
    }

    [HttpGet]
    public ActionResult<ApiResponse<IQueryable<Comment>>> GetAllCommentsTodo(int todoId) {
        ApiResponse<IQueryable<Comment>> response = new();
       Todo? Todo = context.Todos?.Find(todoId);
        if (Todo == null)
        {
            response.Error = "Todo does not exisit";
            return BadRequest(response);
        } 
        response.Data = context.Comments.Where(com => com.TodoId == todoId);
        return response;
    }
}