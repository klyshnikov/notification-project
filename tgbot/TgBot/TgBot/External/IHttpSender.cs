using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBot.Commands.Models;
using TgBot.Models;

namespace TgBot.External;

internal interface IHttpSender
{
    Task<HttpResponseMessage> SendHttpQueryableSimple(string route, HttpRequestType httpRequestType, Dictionary<string, string> values);

    Task<HttpResponseMessage> SendHttpHeadableSimple(string route, HttpRequestType httpRequestType, Dictionary<string, string> values);

}
