using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Commands.Models;

public class GetInvitesResponse
{
    public string UserId { get; set; }

    public string TeamName { get; set; }
}
