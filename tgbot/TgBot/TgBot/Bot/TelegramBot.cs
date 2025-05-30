﻿using System;
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
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using TgBot.Models;
using Microsoft.Extensions.Logging;

namespace TgBot.Bot;

internal class TelegramBot
{
    private readonly TelegramBotClient _client;
    private readonly HttpClient _httpClient;

    internal TelegramBot()
    {
        _httpClient = new HttpClient();
        _client = new TelegramBotClient(SettingsManager.TOCKEN, _httpClient);
        BotHelper.SetClient(_client);
    }

    internal async Task<string> Start()
    {
        foreach ((CommandAvailabilityScope availabilityScope, List<BotCommand> commands) in CommandManager.BotCommands)
        {
            switch (availabilityScope)
            { 
                case CommandAvailabilityScope.Private:
                    await _client.SetMyCommands(commands, BotCommandScope.AllPrivateChats());
                    break;
                case CommandAvailabilityScope.Chat:
                    await _client.SetMyCommands(commands, BotCommandScope.AllGroupChats());
                    Thread.Sleep(1000);
                    break;
            }
        }

        _client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync, new ReceiverOptions { AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery] });

        Thread.Sleep(3000);
        return "Succsesfully start!";
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

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellation)
    {
        if (update.Message is not { } message)
        {
            return;
        }

        if (message.Text is not { } messageText)
        {
            return;
        }

        BotResponse? result = await CommandManager.TryFindCommandAndMaybeExecute(message, new BotOptions(), cancellation).ConfigureAwait(false);

        if (result is not null)
        {
            await _client.SendTextMessageAsync(message.Chat.Id, result.TextMessages[0]).ConfigureAwait(false);
        }

        Console.WriteLine($"Received a '{messageText}' message in chat {message.Chat.Id}.");
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}
