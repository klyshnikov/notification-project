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

[BotCommand("assign_to", "Назначить wi кому-то", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class AssignToCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 3);
        string wiId = commandArgs[1];
        string username = commandArgs[2];

        if (username.Length > 0 && username[0] == '@')
        {
            username = username.Remove(0, 1);
        }

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("assign-to", HttpRequestType.Put, new Dictionary<string, string>
        {
            { "wiId", wiId },
            { "username", username }
        });

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"WorkItem успешно назначен на @{username}")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
