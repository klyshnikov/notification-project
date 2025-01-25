using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Models;

namespace TgBot.Commands;

internal class CommandManager
{
    internal static Dictionary<CommandAvailabilityScope, List<BotCommand>> BotCommands { get; } = GetBotCommands();

    private static Dictionary<CommandAvailabilityScope, List<BotCommand>> GetBotCommands()
    { }
}
