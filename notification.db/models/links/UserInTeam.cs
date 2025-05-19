using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.links;

[Table("user_in_team")]
public class UserInTeam
{
    [Column("user_id", Order = 0)]
    public string UserId { get; set; }

    [Column("team_id", Order = 1)]
    public string TeamId { get; set; }
}
