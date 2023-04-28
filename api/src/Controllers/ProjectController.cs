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

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Error { get; set; } = "";
    }
    [HttpGet()]
    public DbSet<Project>? Get()
    {
        return context.Projects;
    }

    [HttpPost]
    public ActionResult<ApiResponse<Project>> Post([FromBody] ProjectDto projectName)
    {
        ApiResponse<Project> response = new ApiResponse<Project>();
        User user = context.Users.FirstOrDefault(u => u.Id == int.Parse(User.Identity.Name));
        if (user != null)
        {
            if (context.Projects.FirstOrDefault(proj => proj.Name == projectName.Name) == null)
            {
                Project newProject = new Project { Name = projectName.Name, User = user, ShareUsersId = { user.Id }, ShareUsersUsername = { user.Username } };
                context.Projects.Add(newProject);
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

    [HttpPatch("{id}")]
    public string Patch(int id, [FromBody] Project projectFormBody)
    {
        Project? project = context.Projects?.Find(id);
        if (project == null)
        {
            return "project does not exisit";
        }
        project.Name = projectFormBody.Name;
        context.Projects?.Update(project);
        context.SaveChanges();
        return "project updated";
    }


    [HttpDelete("{id}")]
    public string Delete(int id)
    {
        Project? project = context.Projects?.Find(id);
        if (project == null)
        {
            return "project does not exisit";
        }
        context.Projects?.Remove(project);
        context.SaveChanges();
        return "project deleted";
    }

    [HttpPost("userprojects")]
    public IQueryable<Project> getAllProjectOfUser(int userId)
    {
        // int id = int.Parse(User.Identity.Name);
        // Console.WriteLine(id);
        User user = context.Users.Find(userId);
        IQueryable<Project> allUserProjects = context.Projects.Where(p => p.ShareUsersId.Contains(user.Id));
        return allUserProjects;
    }

    [HttpPost("projecttodos")]
    public ActionResult<IQueryable<Todo>> getAllTasksOfPrject(int projectId)
    {
        int userId = int.Parse(User.Identity.Name);
        if (context.Users.Find(userId) != null)
        {
            IQueryable<Todo> allProjectTodos = context.Todos.Where(todo => todo.ProjectId == projectId);
            return Ok(allProjectTodos.OrderBy(t => t.index));
        }
        return new BadRequestObjectResult("User does not exist");
    }

    [HttpPatch("addUserIdToProject")]
    public ActionResult<ApiResponse<string>> addUserIdToProjectPost(int projectId, string sharedUserEmail)
    {
        ApiResponse<string> response = new ApiResponse<string>();
        int userId = int.Parse(User.Identity.Name);
        User user = context.Users.Find(userId);
        User sharedUser = context.Users.FirstOrDefault(u => u.Email == sharedUserEmail);
        if (sharedUser == null)
        {
            response.Error = "couldt find email";
            return BadRequest(response);
        }
        if (user != null)
        {
            Project project = context.Projects.Find(projectId);
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
                            project.ShareUsersUsername.Add(sharedUser.Username);
                            context.Projects.Update(project);
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
        return BadRequest(response);
    }
}