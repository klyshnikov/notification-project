using Microsoft.AspNetCore.Mvc;
using repo;
using models.entity;

namespace notification.db.Controllers;

[ApiController]
[Route("group")]
public class GroupController : ControllerBase
{
    private readonly AppDbContext dbContext;

    [HttpPost("/create-group")]
    public Group CreateGroup(
        [FromBody] long userId,
        [FromBody] string name)
    {

    }

    [HttpPost("/add-group-member")]
    public TeamMember AddGroupeMember(
            [FromBody] string userId,
            [FromBody] string memberId,
            [FromBody] string name)
    { 
        
    }

    [HttpPost("/delete-group-member")]
    public TeamMember DeleteGroupMember(
            [FromBody] string userId,
            [FromBody] string memberId,
            [FromBody] string name)
    { 
        
    }

    [HttpDelete("/delete-group")]
    public Group DeleteGroup(
            [FromBody] string userId,
            [FromBody] string name)
    { 
        
    }
}