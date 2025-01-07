using Microsoft.AspNetCore.Mvc;

using models;
using repo;

namespace notification.db.Controllers;

[ApiController]
[Route("group")]
public class GroupController : ControllerBase
{
    private readonly AppDbContext dbContext;
    
    [HttpPost("/add-group")]
    public Group AddGroup(
        [FromBody] string name,
        [FromBody] long userId,
        [FromBody] List<TeamMember> members)
    {
        
    }
}