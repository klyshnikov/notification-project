using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBot.Models;
using Telegram.Bot.Types;

namespace TgBot.Commands.Interfaces;

internal interface IBotCommand
{
    Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken);
}
