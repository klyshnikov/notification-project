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

[BotCommand("get_my_id", "Посмотреть id пользователя", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class GetMyIdCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        var userId = message.From.Id;
        var httpSender = new HttpSender();

        //var result = await httpSender.SendHttpQueryableSimple("new-user", HttpRequestType.Post, new Dictionary<string, string> { { "userId", userId.ToString() } });

        return await Task.FromResult<BotResponse?>(new BotResponse(userId.ToString())).ConfigureAwait(false);
    }
}
