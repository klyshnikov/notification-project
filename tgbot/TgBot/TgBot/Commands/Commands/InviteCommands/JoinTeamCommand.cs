using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.External;
using TgBot.Models;

namespace TgBot.Commands.Commands.InviteCommands;

[BotCommand("join_team", "Вступить в команду", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class JoinTeamCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 2);
        string teamName = commandArgs[1];
        var userId = message.From.Id;
        var chatId = message.Chat.Id;

        //var teamMembers = await BotHelper.GetChatMembers(chatId);
        Console.WriteLine("inside command");

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("join-team", HttpRequestType.Post, new Dictionary<string, string> { { "userId", userId.ToString() }, { "teamName", teamName } });

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse("Вы успешно вступили в команду")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
