using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBot.Bot;

internal class BotHelper
{
    private static TelegramBotClient _client;

    public static void SetClient(TelegramBotClient client)
    { 
        _client = client;
    }

    public static async Task<ChatMember[]> GetChatMembers(long chatId)
    {
        return await _client.GetChatAdministrators(chatId);
    }
}
