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
using TgBot.Models;
using Microsoft.Extensions.Logging;

namespace TgBot.Commands;

internal class CommandManager
{
    private static readonly char[] _commandDelimiters = { ' ', '\t' };

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
                    AddBotCommand(commandData.Item2, commandName, commandData.Item1, result);
                    break;
                case CommandAvailabilityScope.Chat | CommandAvailabilityScope.Private:
                    AddBotCommand(CommandAvailabilityScope.Chat, commandName, commandData.Item1, result);
                    AddBotCommand(CommandAvailabilityScope.Private, commandName, commandData.Item1, result);
                    break;
            }
        }

        return result;
    }

    private static Dictionary<string, (string, CommandAvailabilityScope, IBotCommand)> GetCommands()
    {
        Console.WriteLine("In GetCommand()");
        var commands = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IBotCommand)))
            .Select(t => (CommandType: t, BotCommand: t.GetCustomAttribute<BotCommandAttribute>()))
            .Where(ta => ta.BotCommand != null)
            .ToDictionary(ta => "/" + ta.BotCommand!.Command, ta => (ta.BotCommand!.Description, ta.BotCommand.AvailabilityScope, (IBotCommand)Activator.CreateInstance(ta.CommandType)!));
        Console.WriteLine(commands);
        return commands;
    }

    private static void AddBotCommand(CommandAvailabilityScope commandAvailabilityScope, string commad, string description, Dictionary<CommandAvailabilityScope, List<BotCommand>> commandsDictionary)
    {
        var c = new BotCommand { Command = commad, Description = description };

        if (commandsDictionary!.TryGetValue(commandAvailabilityScope, out List<BotCommand>? list))
        {
            list.Add(c);
        }
        else 
        {
            commandsDictionary[commandAvailabilityScope] = new List<BotCommand> { c };
        }
    }

    internal static async Task<BotResponse> TryFindCommandAndMaybeExecute(Message message, BotOptions botOptions, CancellationToken cancellationToken)
    {
        string commandText = message.Text;

        int commandDelimiterIndex = commandText.IndexOfAny(_commandDelimiters);

        if (commandDelimiterIndex >= 0)
            commandText = commandText[..commandDelimiterIndex];

        int botNameStartIndex = commandText.IndexOf('@');

        if (botNameStartIndex >= 0)
        {
            commandText = commandText[..botNameStartIndex];
        }

        if (GetCommands().TryGetValue(commandText, out (string, CommandAvailabilityScope, IBotCommand) command))
            return await command.Item3.ExecuteAsync(message, botOptions, cancellationToken).ConfigureAwait(false);

        return null;
    }
}
