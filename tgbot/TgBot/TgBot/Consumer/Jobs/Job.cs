using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBot.Settings;

namespace TgBot.Consumer.Jobs;

internal abstract class Job : IJob
{
    //private readonly ConsumerReader _consumerReader = new ConsumerReader();
    private string _topic;
    private HttpClient _httpClient;
    private TelegramBotClient _client;

    private void InitFields()
    {
        _httpClient = new HttpClient();
        _client = new TelegramBotClient(SettingsManager.TOCKEN, _httpClient);
    }

    public Job(string topic)
    {
        _topic = topic;
        if (_httpClient is null || _client is null)
        {
            InitFields();
        }
    }

    public abstract void ProcessMessage(string message);


    private ConsumerConfig config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "backend",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    public async Task Send(string chatId, string message)
    {
        await _client.SendMessage(long.Parse(chatId), message).ConfigureAwait(false);
    }

    public async Task<string> Start()
    {
        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe(_topic);

            try
            {
                while (true)
                {
                    var result = consumer.Consume(TimeSpan.FromMilliseconds(1000));

                    if (result != null)
                    {
                        ProcessMessage(result.Message.Value);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Потребитель был остановлен, например, через Ctrl+C
                return "Consumer was stopped.";
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}

