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

namespace TgBot.Commands.Commands.WiCommands;

[BotCommand("delete_wi", "Удалить задачу", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class DeleteWiCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 2);
        string wiId = commandArgs[1];

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("delete-wi", HttpRequestType.Delete, new Dictionary<string, string>
        {
            { "wiId", wiId }
        });

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse("Wi удален успешно")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
