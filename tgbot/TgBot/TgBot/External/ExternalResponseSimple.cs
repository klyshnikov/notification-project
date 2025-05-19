using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.External;

internal record class ExternalResponseSimple
{
    public ExternalResponseSimple(string response)
    {
        Response = response;
    }

    public string Response = string.Empty;
}
