using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.UserModel;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly DataContext context;
    private readonly IConfiguration _configuration;

    public UserController(IConfiguration configuration,DataContext context) {
        this._configuration = configuration;
        this.context = context;
    }

    private string userCreateJwt(string id) {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, id)
        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
        SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        JwtSecurityToken token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // [HttpGet]
    // public DbSet<User>? Get() {
    //     return context.Users;
    // }
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Error { get; set; } = "";
    }

    [HttpGet(), Authorize()]
    public ActionResult<ApiResponse<User>> GetInfo() {
        ApiResponse<User> response = new ();
        string id = User.Identity.Name;
        User? user = context.Users?.Find(int.Parse(id));
        if(user == null) {
            response.Error = "user does not exsist";
            return BadRequest(response);
        }
        response.Data = user;
        return response; 
    }
    [HttpPost] 
    public ActionResult<ApiResponse<string>> Post(UserDto userFormBody) {
        ApiResponse<string> response = new ApiResponse<string>();
        if(context.Users?.FirstOrDefault(user => user.Email == userFormBody.Email) == null) {
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(userFormBody.Password);
            User newUser = new User { Username = userFormBody.Username, Email = userFormBody.Email, Password = hashPassword };
            context.Users?.Add(newUser);
            context.SaveChanges();
            response.Data = userCreateJwt(newUser.Id+"");
            return response;
        }

        response.Error = "Email already exist.";
        return response;
    }

    [HttpPost("login")]
    public ActionResult<ApiResponse<string>> post(UserDto userFormBody) {
        ApiResponse<string> response = new ApiResponse<string>();
        User user = context.Users?.FirstOrDefault(user => user.Email == userFormBody.Email);
        if(user == null) {
            response.Error = "The username and/or password you specified are not correct.";
            return BadRequest(response);
        }
        if(!BCrypt.Net.BCrypt.Verify(userFormBody.Password, user.Password)){
            response.Error = "The username and/or password you specified are not correct.";
            return BadRequest(response);
        }
            response.Data = userCreateJwt(user.Id+"");
            return response;
    }

    // [HttpPatch, Authorize]
    // public string Patch([FromBody] User userFormBody) {
    //     string id = User.Identity.Name;
    //     User? user = context.Users?.Find(int.Parse(id));
    //     if(user == null){
    //         return "user does not exisit";
    //     }
    //     user.Username = userFormBody.Username;
    //     context.Users?.Update(user);
    //     context.SaveChanges();
    //     return "user updated";
    // }


    [HttpDelete() ,Authorize()]
    public ApiResponse<string> Delete() {

        ApiResponse<string> response = new ApiResponse<string>();
        int id = int.Parse(User.Identity.Name) ;
        User? user = context.Users.Find(id);
        Console.WriteLine(user.ToString());
        if(user == null){
            response.Error = "user does not exisit"; 
            return response;
        }
        context.Users?.Remove(user);
        context.SaveChanges();
        response.Data = "user deleted";
        return response;
    }
}