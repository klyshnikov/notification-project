using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands;
using TgBot.Commands.Models;
using Telegram.Bot;
using TgBot.Settings;
using Telegram.Bot.Exceptions;

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
                    await RepeatAsync(_client.SetMyCommandsAsync(commands, BotCommandScope.AllPrivateChats()));
                    break;
                case CommandAvailabilityScope.Chat:
                    await RepeatAsync(_client.SetMyCommandsAsync(commands, BotCommandScope.AllGroupChats()));
                    break;
            }
        }


    }

    private async Task RepeatAsync(Task task)
    {
        for (int i = 0; i < SettingsManager.MAX_REPEAT_COUNT_WHILE_USE_TG_API; ++i)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (RequestException e)
            {
                if (i == SettingsManager.MAX_REPEAT_COUNT_WHILE_USE_TG_API)
                {
                    throw;
                }
                else
                { 
                    await Task.Delay(SettingsManager.MAX_REPEAT_COUNT_WHILE_USE_TG_API).ConfigureAwait(false);
                }
            }
        }
    }
}
