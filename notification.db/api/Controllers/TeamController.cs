using Microsoft.AspNetCore.Mvc;
using models.entity;
using repo;

namespace notification.db.Controllers;

[ApiController]
[Route("team")]
public class TeamController : ControllerBase
{
    private readonly AppDbContext dbContext;

    [HttpPost("/create-team")]
    public Group CreateTeam(
        [FromBody] long userId,
        [FromBody] string name)
    {

    }

    [HttpPost("/add-team-member")]
    public TeamMember AddTeamMember(
            [FromBody] string userId,
            [FromBody] string memberId,
            [FromBody] string name)
    {

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