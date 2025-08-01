using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Infrastructure.Security;

// The namespace should match the project and folder structure.
namespace TodoApp.Infrastructure.Data;

/// <summary>
/// The database context for the application, acting as a bridge to the database.
/// It's responsible for querying and saving data.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// This constructor is used by the Dependency Injection container to pass in
    /// configuration options, such as the database connection string.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // --- DbSets ---
    // Each DbSet<T> property maps to a table in the database.

    /// <summary>
    /// Represents the 'Users' table in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Represents the 'TodoItems' table in the database.
    /// </summary>
    public DbSet<TodoItem> TodoItems { get; set; }

    public DbSet<TaskHistory> TaskHistories { get; set; }

    /// <summary>
    /// This method is called by EF Core when the model is first being created.
    /// It's used to configure the entities, relationships, and constraints.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // It's good practice to call the base method.
        base.OnModelCreating(modelBuilder);

        // --- User Entity Configuration ---
        modelBuilder.Entity<User>(builder =>
        {
            // Set the primary key.
            builder.HasKey(u => u.Id);

            // Make the Username property required.
            builder.Property(u => u.Username).IsRequired();

            // Create a unique index on the Username column to prevent duplicate usernames.
            // This is crucial for a login system.
            builder.HasIndex(u => u.Username).IsUnique();

            // Make the HashedPassword property required.
            builder.Property(u => u.HashedPassword).IsRequired();
        });


        // --- TodoItem Entity Configuration ---
        modelBuilder.Entity<TodoItem>(builder =>
        {
            // Set the primary key.
            builder.HasKey(t => t.Id);

            // Make the Title property required.
            builder.Property(t => t.Title).IsRequired();

            // Configure the relationship between TodoItem and User (Creator).
            // A TodoItem has one Creator, and a User can create many TodoItems.
            builder.HasOne(t => t.Creator)
                   .WithMany() // We don't need a navigation property on User for created items.
                   .HasForeignKey(t => t.CreatorId)
                   .OnDelete(DeleteBehavior.Restrict); // IMPORTANT: Prevents deleting a user if they have created tasks.

            // Configure the relationship between TodoItem and User (AssignedTo).
            // A TodoItem has one optional AssignedTo user, and a User can be assigned many TodoItems.
            builder.HasOne(t => t.AssignedTo)
                   .WithMany() // We don't need a navigation property on User for assigned items.
                   .HasForeignKey(t => t.AssignedToId)
                   .IsRequired(false) // This makes the foreign key nullable.
                   .OnDelete(DeleteBehavior.SetNull); // IMPORTANT: If an assigned user is deleted, the task's AssignedToId becomes NULL.

        });

        modelBuilder.Entity<TaskHistory>(builder =>
        {
            builder.HasKey(h => h.Id);

            // To improve query performance, we can still add indexes on the ID columns.
            builder.HasIndex(h => h.TodoItemId);
            builder.HasIndex(h => h.UserId);

            // Explicitly ignore the navigation properties at the database mapping level.
            // This makes our intention crystal clear to EF Core.
            builder.Ignore(h => h.TodoItem);
            builder.Ignore(h => h.User);
        });

        SeedInitialAdminUser(modelBuilder);
    }
    private void SeedInitialAdminUser(ModelBuilder modelBuilder)
    {
        // --- DATA SEEDING ---
        // This code will run when the database is first created by a migration.
        // It pre-populates the database with essential data.

        // Hash the default admin password.
        var adminPasswordHash = PasswordHasher.HashPassword("admin");

        // Seed the default administrator user.
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                HashedPassword = "Vs+y4YmzkPR7FVZjKLtTSQ==;LiLRzmFSgXYlWfLa4XcD+3xtGwSMGlVr9Q8G4bVxlrU=",
                Role = UserRole.Admin
            }
        );
    }
}
