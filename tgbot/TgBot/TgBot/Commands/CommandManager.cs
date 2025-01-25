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
        var result = new Dictionary<CommandAvailabilityScope, List<BotCommand>>();

        foreach ((string commandName, (string, CommandAvailabilityScope, IBotCommand) commandData) in GetCommands())
        {
            switch (commandData.Item2)
            {
                case CommandAvailabilityScope.Chat:
                case CommandAvailabilityScope.Private:
                    AddBotCommand(commandData.Item2, commandName, commandData.Item1);
                    break;
                case CommandAvailabilityScope.Chat | CommandAvailabilityScope.Private:
                    AddBotCommand(CommandAvailabilityScope.Chat, commandName, commandData.Item1);
                    AddBotCommand(CommandAvailabilityScope.Private, commandName, commandData.Item1);
                    break;
            }
        }

        return result;
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
