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

[BotCommand("new_user", "Новый пользователь", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class NewUserCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        var userId = message.From.Id;
        var username = message.From.Username;
        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("new-user", HttpRequestType.Post, new Dictionary<string, string> { { "userId", userId.ToString() }, { "username", username} });

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"@{username}, вы в системе")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
