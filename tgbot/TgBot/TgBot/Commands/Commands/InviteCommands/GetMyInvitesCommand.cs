using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.Commands.Models;
using TgBot.External;
using TgBot.Models;

namespace TgBot.Commands.Commands.InviteCommands;

[BotCommand("get_invites", "Посмотреть все запросы на вступление в команду", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class GetMyInvitesCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        var userId = message.From.Id;
        var chatId = message.Chat.Id;

        //var teamMembers = await BotHelper.GetChatMembers(chatId);
        Console.WriteLine("inside command");

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("get-invites", HttpRequestType.Get,new Dictionary<string, string> { { "userId", userId.ToString() } });
        List<GetInvitesResponse> invites = await result.Content.ReadFromJsonAsync<List<GetInvitesResponse>>();

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Список ваших запросов: \n");
        invites.ForEach(inv => sb.AppendLine($"Пользователь с id = {inv.UserId} отправил запрос на вступление в команду {inv.TeamName}"));

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse(sb.ToString())).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
