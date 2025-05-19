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

[BotCommand("change_wi", "Поменять wi", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class ChangeWiCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 4);
        string wiId = commandArgs[1];
        string fieldName = commandArgs[2];
        string fieldValue = commandArgs[3];

        Console.WriteLine("inside command");

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("change-wi", HttpRequestType.Post, new Dictionary<string, string>
        { 
            { "wiId", wiId }, 
            { "fieldName", fieldName },
            { "fieldValue", fieldValue }
        });

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse("Wi успешно изменен")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
