using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace models.entity;

[Table("work_items")]
public class WorkItem
{
    [Column("id", Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Column("name", Order = 1)]
    public string Name { get; set; }
    [Column("work_item_type", Order = 2)]
    public string WorkItemType { get; set; }

    [Column("created_by", Order = 3)]
    public string CreatedBy { get; set; }

    [Column("created_by_type", Order = 4)]
    public string CreatedByType { get; set; } // Не лучшая идея так делать. Надо думать.

    [Column("assign_to", Order = 5)]
    public string AssignTo { get; set; }

    [Column("assign_to_type", Order = 6)]
    public string AssignToType { get; set; }  // Не лучшая идея так делать. Надо думать.
}