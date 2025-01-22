using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using models.entity;
using models.links

using repo;

namespace notification.db.Controllers;

[ApiController]
[Route("team")]
public class TeamController : ControllerBase
{
    private readonly AppDbContext dbContext;

    [HttpPost("/create-team")]
    public Task<Team> CreateTeam(
        [FromBody] string userId,
        [FromBody] string name)
    {
        Team team = new Team { };

        dbContext.Teams.Add(team);
        dbContext.TeamMemberInTeam.Add(new TeamMemberInTeam { TeamMemberId = userId, TeamId = team.Id });

        return Task.FromResult(team);
    }

    [HttpPost("/add-team-member")]
    public Task<TeamMember> AddTeamMember(
            [FromBody] string userId,
            [FromBody] string memberId,
            [FromBody] string name)
    {   
        FormattableString query = 
            $"select tm.TeamId from TeamMemberInTeam as tm join Team as t on t.Id = tm.TeamId where tm.TeamMemberId = {userId} and t.Name = {name}";

        TeamMember? teamMember = dbContext.TeamMembers.FirstOrDefault(tm => tm.Id == memberId);
        string? teamId = dbContext.Database.SqlQuery<string>(query).FirstOrDefault();

        dbContext.TeamMemberInTeam.Add(new TeamMemberInTeam { TeamMemberId = memberId, TeamId = teamId});

        return Task.FromResult<TeamMember>(teamMember);
    }

    [HttpPost("/delete-team-member")]
    public TeamMember DeleteTeamMember(
            [FromBody] string userId,
            [FromBody] string memberId,
            [FromBody] string name)
    {

    }

    [HttpDelete("/delete-team")]
    public Group DeleteTeam(
            [FromBody] string userId,
            [FromBody] string name)
    {

    }

    [HttpGet("/get-groups")]
    public Group GetGroups(
            [FromBody] string userId,
            [FromBody] string name)
    {

    }
}