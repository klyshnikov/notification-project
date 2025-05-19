using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Commands.Attributes;
using TgBot.Commands.Interfaces;
using TgBot.Commands.Models;
using TgBot.External;
using TgBot.Models;

namespace TgBot.Commands.Commands.WiCommands;

[BotCommand("create_wi", "Создать новую задачу", Models.CommandAvailabilityScope.Chat | Models.CommandAvailabilityScope.Private)]
internal class CreateWiCommand : IBotCommand
{
    public async Task<BotResponse?> ExecuteAsync(Message message, BotOptions botOptions, CancellationToken cancellationTocken)
    {
        Console.WriteLine("CreateWiCommand()");
        string[] commandArgs = message.Text.Split('\n');
        Dictionary<string, string> commandArgsDictionary = new Dictionary<string, string>();
        foreach (var arg in commandArgs.Skip(1))
        {
            var args = ProcessCommandArg(arg);
            commandArgsDictionary.TryAdd(args.Item1, args.Item2);
        }

        commandArgsDictionary = MakeClean(commandArgsDictionary);

        commandArgsDictionary.TryAdd("AuthorId", message.From.Id.ToString());
        commandArgsDictionary.TryAdd("CreatedTime", DateTime.Now.ToString());
        commandArgsDictionary.TryAdd("ChatId", message.Chat.Id.ToString());

        CreateWiRequest createWiRequest = new CreateWiRequest
        {
            Title = commandArgsDictionary["Title"],
            Description = commandArgsDictionary["Description"],
            StartTime = DateTime.Parse(commandArgsDictionary["StartTime"]),
            EndTime = DateTime.Parse(commandArgsDictionary["EndTime"]),
            AuthorId = commandArgsDictionary["AuthorId"],
            CreatedTime = DateTime.Parse(commandArgsDictionary["CreatedTime"]),
            ChatId = commandArgsDictionary["ChatId"]
        };

        var httpSender = new HttpSender();

        var result = await httpSender.SendHttpBodySimple("create-wi", HttpRequestType.Put, createWiRequest);

        if (result.IsSuccessStatusCode)
        {
            return await Task.FromResult<BotResponse?>(new BotResponse("Задача успешно создана")).ConfigureAwait(false);
        }
        else
        {
            return await Task.FromResult<BotResponse?>(new BotResponse($"Ошибка {await result.Content.ReadAsStringAsync()}")).ConfigureAwait(false);
        }
    }

    private (string, string) ProcessCommandArg(string arg)
    {
        Regex regex = new Regex(@"^(\w+)\b(.*)$");
        Match match = regex.Match(arg);
        if (match.Success)
        {
            return (match.Groups[1].Value, match.Groups[2].Value);
        }
        throw new ArgumentException("Bad arguments");
    }

    private Dictionary<string, string> MakeClean(Dictionary<string, string> commandArgsDictionary)
    {
        string title;
        string description;
        string startTimeString;
        string endTimeString;

        if (commandArgsDictionary.TryGetValue("Title", out title)) { }
        if (commandArgsDictionary.TryGetValue("Description", out description)) { }
        if (commandArgsDictionary.TryGetValue("StartTime", out startTimeString)) { }
        if (commandArgsDictionary.TryGetValue("EndTime", out endTimeString)) { }

        return new Dictionary<string, string>() 
        {
            {"Title", title },
            {"Description", description},
            {"StartTime", startTimeString },
            {"EndTime", endTimeString}
        };

    }

    //private DateTime ParseDateTime(string dateTimeString)
    //{
    //    return DateTime.ParseExact(
    //        dateTimeString,
    //        "dd:MM:yyyy HH:mm",
    //        CultureInfo.InvariantCulture
    //    );
    //}


}
