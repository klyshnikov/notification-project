using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands;
using TgBot.Commands.Models;
using Telegram.Bot;

namespace TgBot.Bot;

internal class Bot
{
    private readonly TelegramBotClient _client;
    private readonly HttpClient _httpClient;

    internal Bot()
    { 
        _httpClient = new HttpClient();
        _client = new TelegramBotClient("TOKEN", _httpClient);
    }

    internal async Task<string> Start()
    {
        foreach ((CommandAvailabilityScope availabilityScope, List<BotCommand> commands) in CommandManager.BotCommands)
        {
            switch (availabilityScope)
            { 
                case CommandAvailabilityScope.Private:
                    await RepeatAsync(_client.SetMyCommandsAsync(commands, BotCommandScope.AllChatAdministrators()));
            }
        }
    }

    private async Task RepeatAsync(Task task)
    { 
        
    }
}
