using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBot.Commands.Models;

namespace TgBot.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited =false)]
internal class BotCommandAttribute : Attribute
{
    public string Command { get; }
    public string Description { get; }
    public CommandAvailabilityScope AvailabilityScope { get; }

    public BotCommandAttribute(string command, string description, CommandAvailabilityScope commandAvailabilityScope)
    { 
        Command = command;
        Description = description;
        AvailabilityScope = commandAvailabilityScope;
    }
}
