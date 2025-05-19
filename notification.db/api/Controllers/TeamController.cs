using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using models.entity;
using models.links;
using repo;

namespace notification.db.Controllers;

[ApiController]
[Route("team")]
public class TeamController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public TeamController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Создать команду
    /// </summary>
    /// <param name="userId">Кто создает</param>
    /// <param name="name">Название команды</param>
    /// <returns></returns>
    [HttpPost("/create-team")]
    public Task<Team> CreateTeam(
        [FromQuery] string userId,
        [FromQuery] string name)
    {
        Team team = new Team { Name = name };

        dbContext.Teams.Add(team);
        dbContext.UserInTeam.Add(new UserInTeam { UserId = userId, TeamId = team.Id });

        dbContext.SaveChanges();

        return Task.FromResult(team);
    }

    /// <summary>
    /// Добавить юзера в команду
    /// </summary>
    /// <param name="userId">Кто добавляет</param>
    /// <param name="memberId">Кого добавляет</param>
    /// <param name="name">Название команды</param>
    /// <returns></returns>
    [HttpPost("/add-team-member")]
    public Task<User> AddTeamMember(
            [FromQuery] string userId,
            [FromQuery] string memberId,
            [FromQuery] string name)
    {   
        FormattableString query = 
            $"select ut.team_id as \"Value\" from user_in_team as ut join teams as t on t.id = ut.team_id where ut.user_id = {userId} and t.name = {name}";

        User? teamMember = dbContext.Users.FirstOrDefault(tm => tm.Id == memberId);
        string? teamId = dbContext.Database.SqlQuery<string>(query).FirstOrDefault();

        dbContext.UserInTeam.Add(new UserInTeam { UserId = memberId, TeamId = teamId });

        dbContext.SaveChanges();

        return Task.FromResult<User>(teamMember);
    }

    [HttpPost("/delete-team-member")]
    public Task<User> DeleteTeamMember(
            [FromQuery] string userId,
            [FromQuery] string memberId,
            [FromQuery] string name)
    {
        FormattableString query =
            $"select tm.\"TeamId\" as \"Value\" from \"TeamMemberInTeam\" as tm join \"Teams\" as t on t.\"Id\" = tm.\"TeamId\" where tm.\"TeamMemberId\" = {userId} and t.\"Name\" = {name}";

        User? teamMember = dbContext.Users.FirstOrDefault(tm => tm.Id == memberId);
        string? teamId = dbContext.Database.SqlQuery<string>(query).FirstOrDefault();

        dbContext.UserInTeam.Where(tm => tm.TeamId == teamId && tm.UserId == memberId).ExecuteDelete();

        dbContext.SaveChanges();

        return Task.FromResult<User>(teamMember);
    }

    [HttpDelete("/delete-team")]
    public Task<Team> DeleteTeam(
            [FromQuery] string userId,
            [FromQuery] string name)
    {
        FormattableString query =
            $"select tm.\"TeamId\" as \"Value\" from \"TeamMemberInTeam\" as tm join \"Teams\" as t on t.\"Id\" = tm.\"TeamId\" where tm.\"TeamMemberId\" = {userId} and t.\"Name\" = {name}";

        string? teamId = dbContext.Database.SqlQuery<string>(query).FirstOrDefault();

        Team team = dbContext.Teams.FirstOrDefault(t => t.Id == teamId);

        dbContext.Teams.Where(t => t.Id == teamId).ExecuteDelete();

        dbContext.SaveChanges();

        return Task.FromResult(team);
    }

    /// <summary>
    /// Получить участников команды
    /// </summary>
    /// <param name="userId">Кто получает</param>
    /// <param name="name">Название команды</param>
    /// <returns></returns>
    [HttpGet("/get-members")]
    public List<User> GetMembers(
            [FromQuery] string userId,
            [FromQuery] string name
        )
    {
        FormattableString query =
            $"select u.id from user_in_team as ut join teams as t on t.id = ut.team_id join users as u on u.id = ut.user_id where ut.user_id = {userId} and t.name = {name}";

        var members = dbContext.Database.SqlQuery<User>(query);

        return members.ToList();
    }

    [HttpGet("/get-groups")]
    public Group GetGroups(
            [FromQuery] string userId,
            [FromQuery] string name)
    {
        return null;
    }
}