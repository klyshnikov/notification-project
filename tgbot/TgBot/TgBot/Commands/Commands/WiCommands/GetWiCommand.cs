using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.External;
using TgBot.Models;

namespace TgBot.Commands.Commands.WiCommands;

[BotCommand("get_wi", "Получить информацию о WorkItem", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class GetWiCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 2);
        string wiId = commandArgs[1];

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("get-wi", HttpRequestType.Get, new Dictionary<string, string>
        {
            { "wiId", wiId}
        });

        if (result.IsSuccessStatusCode)
        {
            var wi = await result.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            var sb = new StringBuilder();
            sb.AppendLine("Информация о WorkItem");

            foreach (var key in wi.Keys)
            {
                sb.AppendLine($"{key}: {wi[key]}");
            }

            return await Task.FromResult<BotResponse?>(new BotResponse(sb.ToString())).ConfigureAwait(false);
        }

        return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
    }
}
