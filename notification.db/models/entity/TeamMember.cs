using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace models.entity;

public class TeamMember
{
    [Key]
    public string Id { get; set; }
}