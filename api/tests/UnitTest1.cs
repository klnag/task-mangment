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






namespace tests;

public class UsersControllerTests
{
    private readonly DataContext _context;

    public UsersControllerTests()
    {
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

        User user = new User { Username = "username1", Email = "email1", Password = "pass1"};
        _context.Users.Add(user);
        // _context.Users.Add(new User { Id = 2, Username = "user2", Password = "password2" });
        _context.SaveChanges();
    }

    [Fact]
    public void Test1()
    {

    }
}