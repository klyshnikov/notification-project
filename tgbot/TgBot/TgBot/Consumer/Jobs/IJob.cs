using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Consumer.Jobs;

internal interface IJob
{
    Task<string> Start();
}
