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

[BotCommand("accept_user", "Посмотреть все запросы на вступление в команду", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]

internal class AcceptUserCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 3);
        string userToAcceptId = commandArgs[1];
        string teamName = commandArgs[2];
        //var userId = message.From.Id;
        var chatId = message.Chat.Id;
        var httpSender = new HttpSender();
        var result = await httpSender.SendHttpQueryableSimple("accept-user", HttpRequestType.Post,new Dictionary<string, string> { { "userId", userToAcceptId }, { "teamName", teamName} });

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse("Пользователь принят в команду")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
