using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telegram.Bot.Types;
using TgBot.Commands.Interfaces;
using TgBot.Models;
using TgBot.Commands.Attributes;

namespace TgBot.Commands;

[BotCommand("help", "Информация о боте", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class HelpCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        Console.WriteLine("Command inside");
        return await Task.FromResult<BotResponse?>(new BotResponse("Hello")).ConfigureAwait(false);
    }
}
