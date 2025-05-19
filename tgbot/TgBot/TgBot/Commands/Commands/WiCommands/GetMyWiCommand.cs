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

[BotCommand("get_my_wi", "Получить все WorkItem, назначенные на вас", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class GetMyWiCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string userId = message.From.Id.ToString();

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("get-my-wi", HttpRequestType.Get, new Dictionary<string, string>
        {
            { "userId", userId }
        });

        if (result.IsSuccessStatusCode)
        {
            var wis = await result.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();
            var sb = new StringBuilder();
            sb.AppendLine($"Информация о WorkItem, назанченных на @{message.From.Username}");
            foreach (var wi in wis)
            {
                foreach (var key in wi.Keys)
                {
                    sb.AppendLine($"{key}: {wi[key]}");
                }
                sb.AppendLine("\n");
            }

            return await Task.FromResult<BotResponse?>(new BotResponse(sb.ToString())).ConfigureAwait(false);
        }

        return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
    }
}
