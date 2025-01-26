using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.Models;

namespace TgBot.Commands;

[BotCommand("set_team", "Назначить данный состав телеграм чата в качестве команды.", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class SetTeamCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {

        Console.WriteLine("Command inside");
        return await Task.FromResult<BotResponse?>(new BotResponse("Hello")).ConfigureAwait(false);
    }
}
