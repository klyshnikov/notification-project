using Microsoft.EntityFrameworkCore;
using models.entity;
using models.links;

namespace repo;

public class AppDbContext : DbContext
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<GroupInTeam> GroupInTeam { get; set; }
    public DbSet<TeamMemberInTeam> TeamMemberInTeam { get; set; }
    public DbSet<TeamMemberInGroup> TeamMemberInGroup { get; set; }

    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupInTeam>().HasKey(r => new { r.TeamId, r.GroupId});
        modelBuilder.Entity<TeamMemberInTeam>().HasKey(r => new { r.TeamId, r.TeamMemberId });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=postgres;Username=postgres;Password=postgres");
    }
}