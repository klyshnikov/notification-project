using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using notification.db.Requests;
using models.subscriptions;
using repo;

namespace notification.db.Controllers;

public class SubController : Controller
{
    private readonly AppDbContext dbContext;
    private readonly List<int> defaultMinutesRemaining = new List<int> { 5, 15, 30, 60, 60*2, 60*3, 60*4, 60*6, 60*12, 60*24, 60*24*2};

    public SubController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpPost("/sub-on-wi-assign")]
    public async Task<IActionResult> SubOnWiAssign([FromQuery] string userId)
    {
        dbContext.WiAssignSubs.Add(new WiAssignSub() { UserId = userId });
        dbContext.SaveChanges();
        return Ok();
    }

    [HttpPost("/sub-on-wi-time-remaining")]
    public async Task<IActionResult> SubOnWiTimeRemaining([FromQuery] string userId)
    { 
        string timeRemainingStr = string.Join(",", defaultMinutesRemaining);
        dbContext.WiTimeRemainingSub.Add(new WiTimeRemainingSub() { UserId = userId, MinutesRemaining = timeRemainingStr});
        dbContext.SaveChanges();
        return Ok();
    }
}
