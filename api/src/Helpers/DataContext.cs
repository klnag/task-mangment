namespace src.Helpers;

using Microsoft.EntityFrameworkCore;
using src.Models.ProjectModel;
using src.Models.UserModel;
using src.Models.TodoModel;
using src.Models.CommentModel;

public class DataContext : DbContext
{
    // protected readonly DbContextOptions<DataContext> Configuration;

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // Configuration = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
    //     // connect to postgres with connection string from app settings
        // options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        // options.UseInMemoryDatabase("MyDb");
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        options.UseNpgsql(configuration.GetConnectionString("TestConnection"));

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<Todo>? Todos { get; set; }
    public DbSet<Comment>? Comments { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Project>()
    //        .HasOne(p => p.User)
    //        .WithMany(u => u.Projects)
    //        .HasForeignKey(p => p.Id);
    //}
}