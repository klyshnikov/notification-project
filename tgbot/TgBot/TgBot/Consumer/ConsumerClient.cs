using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBot.Consumer.Jobs;

namespace TgBot.Consumer;

internal class ConsumerClient
{
    private readonly List<IJob> _jobsToProcess = new List<IJob>
    {
        new WiInformingJob("wi_informing"),
        new SimpleJob("wi_assign")
    };

    public async Task<string> Start()
    {
        Console.WriteLine("Start Consumer Client");
        var t1 = Task.Run(() => _jobsToProcess[0].Start());
        var t2 = Task.Run(() => _jobsToProcess[1].Start());
        //var jobsToProcessTasks = _jobsToProcess.Select(j => j.Start()).ToArray();
        await Task.WhenAll(t1, t2);

        return "Success Start() in ConsumerClient";
    }
}
