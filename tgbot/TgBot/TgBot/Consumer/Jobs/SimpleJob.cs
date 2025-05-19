using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Consumer.Jobs;

internal class SimpleJob : Job
{
    public SimpleJob(string topic) : base(topic) { }

    public async override void ProcessMessage(string message)
    {
        try
        {
            Console.WriteLine("ProcessMessage() in SimpleJob");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

            string chatId = values["chatId"];
            string username = values["username"];
            string wiId = values["wiId"];
            string wiTitle = values["wiTitle"];



            await Send(chatId, $"@{username}, на вас поступил новый WorkItem.\nId = {wiId}, Tittle = {wiTitle}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
