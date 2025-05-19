using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.subscriptions;

[Table("wi_time_remaining_sub")]
public class WiTimeRemainingSub
{
    [Column("user_id", Order = 0)]
    public string UserId { get; set; }

    [Column("minutes_remaining", Order = 1)]
    public string MinutesRemaining { get; set; }
}
