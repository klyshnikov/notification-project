using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TgBot.Bot;

namespace TgBot;

public static class Program
{
    public static void Main(string[] args)
    {
        Environment.CurrentDirectory = AppContext.BaseDirectory;

        var _bot = new TelegramBot();
        Task.FromResult(_bot.Start());
        Console.ReadLine();
    }
}

