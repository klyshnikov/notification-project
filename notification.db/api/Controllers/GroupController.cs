using Microsoft.AspNetCore.Mvc;
using repo;
using models.entity;

namespace notification.db.Controllers;

[ApiController]
[Route("group")]
public class GroupController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public GroupController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpPost("/create-group")]
    public Group CreateGroup(
        [FromQuery] long userId,
        [FromQuery] string name)
    {
        return null;
    }

    [HttpPost("/add-group-member")]
    public TeamMember AddGroupeMember(
            [FromQuery] string userId,
            [FromQuery] string memberId,
            [FromQuery] string name)
    {
        return null;    
    }

    [HttpPost("/delete-group-member")]
    public TeamMember DeleteGroupMember(
            [FromQuery] string userId,
            [FromQuery] string memberId,
            [FromQuery] string name)
    {
        return null;
    }

    [HttpDelete("/delete-group")]
    public Group DeleteGroup(
            [FromQuery] string userId,
            [FromQuery] string name)
    {
        return null;
    }
}