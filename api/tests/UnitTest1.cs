using src.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using src.Controllers;
using src.Models.UserModel;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public string Error { get; set; } = "";
}


namespace tests{

public class UsersControllerTests
{
    private readonly DataContext _context;
    private readonly UserController userController;
    private readonly HttpClient _client;

        public UsersControllerTests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5242"); // replace with your API's base URL
        
            // var options = new DbContextOptionsBuilder<DataContext>()
            // .
            //     // .UseInMemoryDatabase(databaseName: "MyDb")
            //     .Options;
            // _context = new DataContext(options);
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("TestConnection"));

            _context = new DataContext(optionsBuilder.Options);
            // var configuration = new ConfigurationBuilder()
            //             .AddJsonFile("appsettings.json")
            //             .Build();

            // var dbContext = new DataContext(optionsBuilder.Options);
            userController = new UserController(configuration, _context);

            // User user = new User { Username = "Test", Email = "test@test.com", Password = "testpassowrd" };
            // _context.Users.Add(user);
            // _context.SaveChanges();
        }

        [Fact(DisplayName = "Create new user")]
        public async void CreateNewUser()
        {
            UserDto user = new UserDto { Username = "usernamse2", Email = "emfddadsiffadl1", Password = "pass1" };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var token = "my-auth-token";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/User", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
        }
//     [Fact]
//     public void DeleteUser() {
// //  var user = new UserDto { Username = "usernamse2", Email = "emfddadsiffdl1", Password = "pass1" };

//         // Act
//         var result = userController.Delete(user);

//         if (result.Value.Error != "")
//         {
//             Console.WriteLine(result.Value.Error);
//         }
//         else
//         {
//             Console.WriteLine(result.Value.Data);
//         }
//     }
}
}