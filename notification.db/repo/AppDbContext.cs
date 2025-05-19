using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using models.entity;
using models.links;
using models.subscriptions;
using System.Security.Principal;
using System.Xml;

namespace repo;

public class AppDbContext : DbContext
{
    // Entity
    public DbSet<Group> Groups { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    //Links
    public DbSet<GroupInTeam> GroupInTeam { get; set; }
    public DbSet<UserInTeam> UserInTeam { get; set; }
    public DbSet<UserInGroup> UserInGroup { get; set; }
    public DbSet<InviteTeam> InviteTeam { get; set; }

    //Subscribitions
    public DbSet<WiAssignSub> WiAssignSubs { get; set; }
    public DbSet<WiTimeRemainingSub> WiTimeRemainingSub { get; set; }

    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Entity
        //modelBuilder.Entity<Team>().ToTable("team"); // Имя таблицы без кавычек
        //modelBuilder.Entity<Team>().Property(e => e.MyColumn).HasColumnName("Column"); // Имя столбца без кавычек
        modelBuilder.Entity<Team>().HasKey(t => t.Id);
        modelBuilder.Entity<Group>().HasKey(t => t.Id);
        modelBuilder.Entity<Role>().HasKey(r => r.Id);
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        //Links
        modelBuilder.Entity<GroupInTeam>().HasKey(r => new { r.TeamId, r.GroupId});
        modelBuilder.Entity<UserInTeam>().HasKey(r => new { r.TeamId, r.UserId });
        modelBuilder.Entity<InviteTeam>().HasKey(r => new { r.UserId, r.TeamId });
        modelBuilder.Entity<UserTeamRole>().HasKey(r => new { r.UserId, r.TeamId });

        //Subscribitions
        modelBuilder.Entity<WiAssignSub>().HasKey(r => new { r.UserId });
        modelBuilder.Entity<WiTimeRemainingSub>().HasKey(r => new { r.UserId });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=postgres;Username=postgres;Password=postgres");
    }
}