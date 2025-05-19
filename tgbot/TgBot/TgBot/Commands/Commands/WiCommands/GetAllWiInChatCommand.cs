using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.External;
using TgBot.Models;

namespace TgBot.Commands.Commands.WiCommands;

[BotCommand("get_all_wi_in_chat", "Получить все созданные задачи для команды в этом чате", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class GetAllWiInChatCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 1);

        string chatId = message.Chat.Id.ToString();

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("get-all-wi-in-chat", HttpRequestType.Get, new Dictionary<string, string>
        {
            { "chatId", chatId }
        });

        if (result.IsSuccessStatusCode)
        {
            var wis = await result.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();
            var sb = new StringBuilder();
            sb.AppendLine("Задачи в чате:");
            foreach (var wi in wis)
            {
                if (wi.ContainsKey("WiId"))
                {
                    sb.AppendLine($"WorkItem с Id = {wi["WiId"]}");
                }

                if (wi.ContainsKey("Title"))
                {
                    sb.AppendLine($"Title: {wi["Title"]}");
                }

                sb.AppendLine("\n\n");
            }

            return await Task.FromResult<BotResponse?>(new BotResponse(sb.ToString())).ConfigureAwait(false);
        }

        return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
    }
}
