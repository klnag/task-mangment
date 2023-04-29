using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.ProjectModel;
using src.Models.UserModel;
using src.Models.TodoModel;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly DataContext context;

    public ProjectController(DataContext context)
    {
        this.context = context;
    }

    // [swaggerid("StringApiResponse")]
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Error { get; set; } = "";
    }
    // [HttpGet()]
    // public DbSet<Project>? Get()
    // {
    //     return context.Projects;
    // }

    [HttpPost("{userId}")]
    public ActionResult<ApiResponse<Project>> Post(int userId,[FromBody] ProjectDto projectName)
    {
        ApiResponse<Project> response = new ApiResponse<Project>();
        // int userId = int.Parse(User.Identity?.Name!);
        User user = context.Users?.Find(userId)!;
        if (user != null)
        {
            if (context.Projects?.FirstOrDefault(proj => proj.Name == projectName.Name) == null)
            {
                Project newProject = new Project { Name = projectName.Name, User = user, ShareUsersId = { user.Id }, ShareUsersUsername = { user.Username! } };
                context.Projects?.Add(newProject);
                context.SaveChanges();
                response.Data = newProject;
                return response;
            }
            response.Error = "project already exists";
            return BadRequest(response);
        }
        response.Error = "user not found";
        return BadRequest(response);
    }

    // [HttpPatch("{id}")]
    // public string Patch(int id, [FromBody] Project projectFormBody)
    // {
    //     Project? project = context.Projects?.Find(id);
    //     if (project == null)
    //     {
    //         return "project does not exisit";
    //     }
    //     project.Name = projectFormBody.Name;
    //     context.Projects?.Update(project);
    //     context.SaveChanges();
    //     return "project updated";
    // }


    // [HttpDelete("{id}")]
    // public string Delete(int id)
    // {
    //     Project? project = context.Projects?.Find(id);
    //     if (project == null)
    //     {
    //         return "project does not exisit";
    //     }
    //     context.Projects?.Remove(project);
    //     context.SaveChanges();
    //     return "project deleted";
    // }

    [HttpGet("getAllUserProjects")]
    public ActionResult<ApiResponse<IQueryable<Project>>> getAllUserProjects(int userId)
    {
        ApiResponse<IQueryable<Project>> response = new();
        if (context.Users?.Find(userId) == null)
        {
            response.Error = "User does not exist";
            return BadRequest(response);
        }
        User? user = context.Users.Find(userId);
        IQueryable<Project> allUserProjects = context.Projects!.Where(p => p.ShareUsersId.Contains(userId));
        response.Data = allUserProjects;
        return response;
    }

    [HttpGet("getAllProjecTodo")]
    public ActionResult<ApiResponse<IQueryable<Todo>>> getAllProjecTodo(int projectId)
    {
        ApiResponse<IQueryable<Todo>> response = new();
        int userId = int.Parse(User.Identity?.Name!);
        if (context.Users?.Find(userId) == null)
        {
            response.Error = "User does not exist";
            return response;
        }

        IQueryable<Todo> allProjectTodos = context.Todos!.Where(todo => todo.ProjectId == projectId);
        response.Data = allProjectTodos.OrderBy(t => t.index);
        return response;
    }

    [HttpPost("addUserToProject")]
    public ActionResult<ApiResponse<string>> addUserToProject(int projectId, string sharedUserEmail)
    {
        ApiResponse<string> response = new();
        int userId = int.Parse(User.Identity?.Name!);
        User user = context.Users?.Find(userId)!;
        User sharedUser = context.Users?.FirstOrDefault(u => u.Email == sharedUserEmail)!;
        if (sharedUser == null)
        {
            response.Error = "couldt find email";
            return BadRequest(response);
        }
        if (user != null)
        {
            Project project = context.Projects?.Find(projectId)!;
            if (project != null)
            {
                if (project.UserId == user.Id)
                {
                    if (sharedUser != null)
                    {
                        // var p = project.ShareUsersId.FirstOrDefault(p => p.Id == sharedUser.Id);
                        if (!project.ShareUsersId.Any(p => p == sharedUser.Id))
                        {
                            project.ShareUsersId.Add(sharedUser.Id);
                            project.ShareUsersUsername.Add(sharedUser.Username!);
                            context.Projects?.Update(project);
                            context.SaveChanges();
                            response.Data = "Done";
                            return response;
                        }
                        response.Error = "id alredy exist";
                        return BadRequest(response);
                    }
                    else
                    {
                        response.Error = "the shared user does not exist";
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Error = "Project does not belongs to this user";
                    return BadRequest(response);
                }
            }
            else
            {
                response.Error = "Project does not exist";
                return BadRequest(response);
            }
        }
        response.Error = "User Does not exisit";
        return response;
    }
}