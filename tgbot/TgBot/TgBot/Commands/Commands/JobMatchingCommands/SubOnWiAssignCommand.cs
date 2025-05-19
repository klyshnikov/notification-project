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

namespace TgBot.Commands.Commands.JobMatchingCommands;

[BotCommand("sub_on_wi_assign", "Подписаться на назначение вам WorkItem", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class SubOnWiAssignCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string userId = message.From.Id.ToString();

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("sub-on-wi-assign", HttpRequestType.Post, new Dictionary<string, string>
        {
            { "userId", userId }
        });

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Подписка оформлена успешно")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
