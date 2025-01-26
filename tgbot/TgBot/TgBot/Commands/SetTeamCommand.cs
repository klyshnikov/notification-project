using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.Models;
using TgBot.Settings;

namespace TgBot.Commands;

[BotCommand("set_team", "Назначить данный состав телеграм чата в качестве команды.", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class SetTeamCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 2);
        string teamName = commandArgs[0];
        var userId = message.From;
        Console.WriteLine("Command inside");

        using HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("Team-Members", teamMembersIds);

        httpClient.BaseAddress = new Uri(SettingsManager.SERVICE_ADDRESS);

        var res = httpClient.GetFromJsonAsync<>("/set-team");

        return await Task.FromResult<BotResponse?>(new BotResponse("Для данной группы успешно создана команда")).ConfigureAwait(false);
    }
}
