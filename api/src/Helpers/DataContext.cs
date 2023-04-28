namespace src.Helpers;

using Microsoft.EntityFrameworkCore;
using src.Models.ProjectModel;
using src.Models.UserModel;
using src.Models.TodoModel;
using src.Models.CommentModel;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
    }

    public DbSet<User>? Users { get; set; }
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