using Microsoft.AspNetCore.Mvc;
using models.entity;
using notification.db.Requests;
using repo;
using models.links;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using notification.db.Responses;

namespace notification.db.Controllers;

[ApiController]
[Route("invite")]
public class InviteController : ControllerBase
{
	private readonly AppDbContext dbContext;

	public InviteController(AppDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="UserId">Кто отправляет запрос на вступление</param>
	/// <param name="TeamName">Название команды, куда отправляется запрос</param>
	/// <returns></returns>
	[HttpPost("/join-team")]
	public async Task<IActionResult> JoinTeam(
		[FromQuery] string userId, [FromQuery] string teamName)
	{
        string teamId = dbContext.Teams.FirstOrDefault(t => t.Name == teamName).Id;
        dbContext.InviteTeam.Add(new InviteTeam { UserId = userId, TeamId = teamId });

        dbContext.SaveChanges();
        return Ok();
    }

	/// <summary>
	/// Получить все приглашения для человека
	/// </summary>
	/// <param name="UserId">Кто отправляет запрос</param>
	/// <returns></returns>
	[HttpGet("/get-invites")]
	public async Task<List<GetInvitesResponse>> GetInvites([FromQuery] string userId)
	{
		FormattableString query = $"select it.user_id as user_id, t.name as team_name from user_in_team as ut join invite_team as it on ut.team_id = it.team_id join teams t on t.id = it.team_id where ut.user_id = {userId}";
		List<GetInvitesResponse> invites = dbContext.Database.SqlQuery<GetInvitesResponse>(query).ToList();
		dbContext.SaveChanges();
		return invites;
	}

	/// <summary>
	/// Принять пользователя в команду
	/// </summary>
	/// <param name="UserId">Пользователь (запрашиваемый)</param>
	/// <param name="TeamId">Куда запрашивает (команда)</param>
	/// <returns></returns>
	[HttpPost("/accept-user")]
	public async Task<IActionResult> AcceptUser([FromQuery] string userId, [FromQuery] string teamName)
	{
		var team = dbContext.Teams.FirstOrDefault(t => t.Name == teamName);
		var inviteToDelete = dbContext.InviteTeam.FirstOrDefault(it => it.UserId == userId && it.TeamId == team.Id);

		if (inviteToDelete is not null)
		{
			dbContext.InviteTeam.Remove(inviteToDelete);
		}
        dbContext.SaveChanges();

        var teamId = dbContext.Teams.FirstOrDefault(t => t.Name == teamName).Id;
		dbContext.UserInTeam.Add(new UserInTeam() { UserId = userId, TeamId = teamId});
		dbContext.SaveChanges();

		return Ok();
	}

	[HttpPut("/set-team")]
	public async Task<IActionResult> SetTeam([FromQuery] string userId, [FromQuery] string teamName, [FromQuery] string chatId)
	{
		EntityEntry<Team> writtenTeam = dbContext.Teams.Add(new Team() { Name = teamName, ChatId = chatId});
        dbContext.SaveChanges();
        dbContext.UserInTeam.Add(new UserInTeam() { UserId = userId, TeamId = writtenTeam.Entity.Id});
		dbContext.SaveChanges();
		return Ok();
	}

	[HttpPost("/new-user")]
	public async Task<IActionResult> NewUser([FromQuery] string userId, [FromQuery] string username)
	{
		dbContext.Users.Add(new User() { Id = userId, Username = username });
		dbContext.SaveChanges();
		return Ok();
	}
}
