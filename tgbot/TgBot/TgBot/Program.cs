using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TgBot.Bot;
using TgBot.Consumer;

namespace TgBot;

public static class Program
{
    public static void Main(string[] args)
    {
        Environment.CurrentDirectory = AppContext.BaseDirectory;

        var _bot = new TelegramBot();
        var botTask = _bot.Start();

        var _consumer = new ConsumerClient();
        var consumerTask = _consumer.Start();

        Task.WaitAll(botTask, consumerTask);
        Console.ReadLine();
    }
}

