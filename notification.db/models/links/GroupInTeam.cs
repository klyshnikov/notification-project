﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.links;

[Table("group_in_team")]
public class GroupInTeam
{
    [Column("team_id", Order = 0)]
    public string TeamId { get; set; }

    [Column("group_id", Order = 1)]
    public string GroupId { get; set; }
}
