using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace models.entity;

[Table("teams")]
public class Team
{
    [Column("id", Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    
    [Column("name", Order = 1)]
    public string Name { get; set; }

    [Column("chat_id", Order = 2)]
    public string ChatId { get; set; }
}