using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Commands.Models;

[Flags]
internal enum CommandAvailabilityScope
{
    None = 0,
    Private = 1,
    Chat = 2,
    All = Private | Chat
}
