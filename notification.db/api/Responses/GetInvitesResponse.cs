using System.ComponentModel.DataAnnotations.Schema;

namespace notification.db.Responses;

public class GetInvitesResponse
{
    [Column("user_id", Order = 0)]
    public string UserId { get; set; }

    [Column("team_name", Order = 1)]
    public string TeamName { get; set; }
}
