using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.subscriptions;

[Table("wi_assign_sub")]
public class WiAssignSub
{
    [Column("user_id", Order = 0)]
    public string UserId { get; set; }
}
