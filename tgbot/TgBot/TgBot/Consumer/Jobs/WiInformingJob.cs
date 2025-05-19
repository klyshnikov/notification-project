using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Json.Net;
using Newtonsoft.Json;

namespace TgBot.Consumer.Jobs;

internal class WiInformingJob : Job
{
    public WiInformingJob(string topic) : base(topic) { }

    public async override void ProcessMessage(string message)
    {
        try
        {
            Console.WriteLine("ProcessMessage() in WiInformingJob");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

            string chatId = values["chatId"];
            string username = values["username"];
            List<string> wiIds = JsonConvert.DeserializeObject<List<string>>(values["wiId"]);
            List<string> wiTitles = JsonConvert.DeserializeObject<List<string>>(values["wiTitle"]);
            List<DateTime> endTimes = JsonConvert.DeserializeObject<List<DateTime>>(values["endTime"]);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"@{username}, Информация о ваших WorkItem \n");

            for (int i = 0; i < wiIds.Count; i++)
            {
                if (endTimes[i] > DateTime.Now)
                {
                    var timeLeft = endTimes[i] - DateTime.Now;
                    sb.AppendLine($"Id: {wiIds[i]}");
                    sb.AppendLine($"Title: {wiTitles[i]}");
                    sb.AppendLine($"Осталось времени: {timeLeft.Days} дней, {timeLeft.Hours} часов, {timeLeft.Minutes} минут");
                    sb.AppendLine($"\n");
                }
                else
                {
                    sb.AppendLine($"У вас имеется просорченный WorkItem");
                    sb.AppendLine($"Id: {wiIds[i]}");
                    sb.AppendLine($"Title: {wiTitles[i]}");
                    sb.AppendLine($"\n");
                }
            }

            await Send(chatId, sb.ToString());
        }
        catch (Exception ex)
        { 
            Console.WriteLine(ex.Message);
        }
        
    }
}
