using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;
using TgBot.Commands.Models;
using TgBot.Models;
using TgBot.Settings;

namespace TgBot.External;

internal class HttpSender : IHttpSender
{
	public HttpSender() { }

	public async Task<HttpResponseMessage> SendHttpQueryableSimple(string endpoint, HttpRequestType httpRequestType, Dictionary<string, string> values)
	{
		string requestString = endpoint + "?" + string.Join('&', values.ToList().Select(kvPair => $"{kvPair.Key}={kvPair.Value}").ToArray());

		using HttpClient httpClient = new HttpClient();
		HttpResponseMessage response;


		switch (httpRequestType)
		{
			case HttpRequestType.Get:
				response = await httpClient.GetAsync($"{SettingsManager.SERVICE_ADDRESS}/{requestString}");
				break;
			case HttpRequestType.Post:
				response = await httpClient.PostAsync($"{SettingsManager.SERVICE_ADDRESS}/{requestString}", null);
				break;
			case HttpRequestType.Put:
				response = await httpClient.PutAsync($"{SettingsManager.SERVICE_ADDRESS}/{requestString}", null);
				break;
			case HttpRequestType.Delete:
				response = await httpClient.DeleteAsync($"{SettingsManager.SERVICE_ADDRESS}/{requestString}");
				break;
			default:
				throw new ArgumentException("httpRequestType is wrong parameter");
		}

		return response;
	}

	public async Task<HttpResponseMessage> SendHttpHeadableSimple(string endpoint, HttpRequestType httpRequestType, Dictionary<string, string> values)
	{
		using HttpClient httpClient = new HttpClient();

		values.ToList().ForEach(kvPair => httpClient.DefaultRequestHeaders.Add(kvPair.Key, kvPair.Value));

        HttpResponseMessage response;

        switch (httpRequestType)
        {
            case HttpRequestType.Get:
                response = await httpClient.GetAsync($"{SettingsManager.SERVICE_ADDRESS}");
                break;
            case HttpRequestType.Post:
                response = await httpClient.PostAsync($"{SettingsManager.SERVICE_ADDRESS}", null);
                break;
            case HttpRequestType.Put:
                response = await httpClient.PutAsync($"{SettingsManager.SERVICE_ADDRESS}", null);
                break;
            case HttpRequestType.Delete:
                response = await httpClient.DeleteAsync($"{SettingsManager.SERVICE_ADDRESS}");
                break;
            default:
                throw new ArgumentException("httpRequestType is wrong parameter");
        }

        return response;
    }

    public async Task<HttpResponseMessage> SendHttpBodySimple(string endpoint, HttpRequestType httpRequestType, object body)
    {
        using HttpClient httpClient = new HttpClient();

        string jsonContent = JsonSerializer.Serialize(body, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response;

        switch (httpRequestType)
        {
            case HttpRequestType.Post:
                response = await httpClient.PostAsync($"{SettingsManager.SERVICE_ADDRESS}/{endpoint}", httpContent);
                break;
            case HttpRequestType.Put:
                response = await httpClient.PutAsync($"{SettingsManager.SERVICE_ADDRESS}/{endpoint}", httpContent);
                break;
            default:
                throw new ArgumentException("httpRequestType is wrong parameter. Should not be Get() or Delete() in SendHttpBodySimple()");
        }

        return response;
    }

}
