using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.links;

public class TeamMemberInTeam
{
    public string TeamMemberId { get; set; }

    public string TeamId { get; set; }
}
