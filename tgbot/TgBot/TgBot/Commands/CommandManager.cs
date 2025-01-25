using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Models;
using TgBot.Commands.Interfaces;
using System.Reflection;
using System.Data;
using TgBot.Commands.Attributes;

namespace TgBot.Commands;

internal class CommandManager
{
    internal static Dictionary<CommandAvailabilityScope, List<BotCommand>> BotCommands { get; } = GetBotCommands();

    private static Dictionary<CommandAvailabilityScope, List<BotCommand>> GetBotCommands()
    { 
        
    }

    private static Dictionary<string, (string, CommandAvailabilityScope, IBotCommand)> GetCommands()
    { 
        return Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IBotCommand)))
            .Select(t => (CommandType: t, BotCommand: t.GetCustomAttribute<BotCommandAttribute>()))
            .Where(ta => ta.BotCommand != null)
            .ToDictionary(ta => "/" + ta.BotCommand!.Command, ta => (ta.BotCommand!.Description, ta.BotCommand.AvailabilityScope, (IBotCommand) Activator.CreateInstance(ta.CommandType)!))
    }
}
