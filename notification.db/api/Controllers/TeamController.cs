﻿using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("/create-team")]
    public Task<Team> CreateTeam(
        [FromQuery] string userId,
        [FromQuery] string name)
    {
        Team team = new Team { Name = name};

        dbContext.Teams.Add(team);
        dbContext.TeamMemberInTeam.Add(new TeamMemberInTeam { TeamMemberId = userId, TeamId = team.Id });

        dbContext.SaveChanges();

        return Task.FromResult(team);
    }

    [HttpPost("/add-team-member")]
    public Task<TeamMember> AddTeamMember(
            [FromQuery] string userId,
            [FromQuery] string memberId,
            [FromQuery] string name)
    {   
        FormattableString query = 
            $"select tm.\"TeamId\" as \"Value\" from \"TeamMemberInTeam\" as tm join \"Teams\" as t on t.\"Id\" = tm.\"TeamId\" where tm.\"TeamMemberId\" = {userId} and t.\"Name\" = {name}";

        TeamMember? teamMember = dbContext.TeamMembers.FirstOrDefault(tm => tm.Id == memberId);
        string? teamId = dbContext.Database.SqlQuery<string>(query).FirstOrDefault();

        dbContext.TeamMemberInTeam.Add(new TeamMemberInTeam { TeamMemberId = memberId, TeamId = teamId });

        dbContext.SaveChanges();

        return Task.FromResult<TeamMember>(teamMember);
    }

    [HttpPost("/delete-team-member")]
    public Task<TeamMember> DeleteTeamMember(
            [FromQuery] string userId,
            [FromQuery] string memberId,
            [FromQuery] string name)
    {
        FormattableString query =
            $"select tm.\"TeamId\" as \"Value\" from \"TeamMemberInTeam\" as tm join \"Teams\" as t on t.\"Id\" = tm.\"TeamId\" where tm.\"TeamMemberId\" = {userId} and t.\"Name\" = {name}";

        TeamMember? teamMember = dbContext.TeamMembers.FirstOrDefault(tm => tm.Id == memberId);
        string? teamId = dbContext.Database.SqlQuery<string>(query).FirstOrDefault();

        dbContext.TeamMemberInTeam.Where(tm => tm.TeamId == teamId && tm.TeamMemberId == memberId).ExecuteDelete();

        dbContext.SaveChanges();

        return Task.FromResult<TeamMember>(teamMember);
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

    [HttpGet("/get-members")]
    public List<TeamMember> GetMembers(
            [FromQuery] string userId,
            [FromQuery] string name
        )
    {
        FormattableString query =
            $"select m.\"Id\" from \"TeamMemberInTeam\" as tm join \"Teams\" as t on t.\"Id\" = tm.\"TeamId\" join \"TeamMembers\" as m on m.\"Id\" = tm.\"TeamMemberId\" where tm.\"TeamMemberId\" = {userId} and t.\"Name\" = {name}";

        var members = dbContext.Database.SqlQuery<TeamMember>(query);

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