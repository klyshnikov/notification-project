using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.links;

[Table("user_in_group")]
public class UserInGroup
{
    [Column("id", Order = 0)]
    public string Id { get; set; }
}
