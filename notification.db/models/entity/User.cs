using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace models.entity;

[Table("users")]
public class User
{
    [Column("id", Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Column("username", Order = 1)]
    public string Username { get; set; }
}