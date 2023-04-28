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

    [HttpPost]
    public ActionResult<Comment> Post([FromBody] CommentDto CommentFormBody)
    {
        Todo todo = context.Todos.FirstOrDefault(u => u.Id == CommentFormBody.TodoId);
        if (todo != null)
        {
            
                Comment newComment = new Comment { Context = CommentFormBody.Context, UserId = int.Parse(User.Identity.Name), TodoId = CommentFormBody.TodoId, UserName = CommentFormBody.UserName };
                context.Comments.Add(newComment);
                context.SaveChanges();
                return newComment;
        }
        return new BadRequestObjectResult("todo not found");
    }

    [HttpGet]
    public ActionResult<IQueryable<Comment>> GetAllCommentsTodo(int todoId) {
       Todo? Todo = context.Todos?.Find(todoId);
        if (Todo != null)
        {
            return Ok(context.Comments.Where(com => com.TodoId == todoId));
        } 
        return new BadRequestObjectResult("Todo does not exisit");
    }
}