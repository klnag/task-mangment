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
    private  string token = "";

        public UsersControllerTests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5242"); // replace with your API's base URL
        
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("TestConnection"));

            _context = new DataContext(optionsBuilder.Options);

            userController = new UserController(configuration, _context);

        }

        [Fact(DisplayName = "Create new user")]
        public async void CreateNewUser()
        {
            UserDto user = new UserDto { Username = "111", Email = "emddddaas2dd2s2cfdgdsadsiddffadl1", Password = "pass1" };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            // var token = "my-auth-token";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/User", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var  responseContentJson = JsonConvert.DeserializeObject<ApiResponse<string>>(responseContent) ;
            token = responseContentJson.Data ;
            // Console.WriteLine(responseContentJson.Error);
        }
    [Fact]
        public async void DeleteUser()
        {
            // Console.WriteLine(token);
            // // var body = new { };
            // // var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync("/api/User");
            // // response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            ApiResponse<string> responseContentJson = JsonConvert.DeserializeObject<ApiResponse<string>>(responseContent);
            Console.WriteLine(responseContent);
        }
}
}