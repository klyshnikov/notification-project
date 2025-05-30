using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace models.entity;

[Table("groups")]
public class Group
{
    [Column("id", Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Column("name", Order = 1)]
    public string Name { get; set; }
}