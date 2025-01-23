using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace models.entity;

public class WorkItem
{
    [Key, Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string WorkItemType { get; set; }

    public string CreatedBy { get; set; }
    public string CreatedByType { get; set; } // �� ������ ���� ��� ������. ���� ������.

    public string AssignTo { get; set; }
    public string AssignToType { get; set; }  // �� ������ ���� ��� ������. ���� ������.
}