using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Bot;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.External;
using TgBot.Models;
using TgBot.Settings;

namespace TgBot.Commands.Commands.InviteCommands;

[BotCommand("set_team", "Назначить данный состав телеграм чата в качестве команды.", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class SetTeamCommand : IBotCommand
{

    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        string[] commandArgs = message.Text.Split(' ', 2);
        string teamName = commandArgs[1];
        var userId = message.From.Id;
        var chatId = message.Chat.Id;

        //var teamMembers = await BotHelper.GetChatMembers(chatId);
        Console.WriteLine("inside command");

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpQueryableSimple("set-team", HttpRequestType.Put, new Dictionary<string, string> { { "userId", userId.ToString() }, { "teamName", teamName }, { "chatId", chatId.ToString() } });

        //using HttpClient httpClient = new HttpClient();

        //httpClient.DefaultRequestHeaders.Add("Team-Members", teamMembersIds);

        //httpClient.BaseAddress = new Uri(SettingsManager.SERVICE_ADDRESS);

        //var res = httpClient.GetFromJsonAsync<>("/set-team");

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse("Команда установлена")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }
}
