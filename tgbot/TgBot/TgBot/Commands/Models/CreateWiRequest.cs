using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Commands.Models;

public class CreateWiRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string AuthorId { get; set; }

    public DateTime CreatedTime { get; set; }

    public string ChatId { get; set; }
}
