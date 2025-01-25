using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Models;

internal class BotResponse
{
    internal const char MessageThreadIdDelimiter = '/';

    private readonly (string, string)[] _inlineKeyboardKeysWithCallbackDataPatterns;

    internal string[] TextMessages { get; }

    internal BotResponse(string textMessage, params (string, string)[] inlineKeyboardKeysWithCallbackDataPatterns) : this([textMessage], inlineKeyboardKeysWithCallbackDataPatterns) { }

    internal BotResponse(string[] textMessage, params (string, string)[] inlineKeyboardKeysWithCallbackDataPatterns)
    { 
        TextMessages = textMessage;
        _inlineKeyboardKeysWithCallbackDataPatterns = inlineKeyboardKeysWithCallbackDataPatterns;
    }
}
