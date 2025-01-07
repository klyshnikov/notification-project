namespace dto;

public class Group {
    private Team team;
    private List<TeamMember> members;

    public Group(Team team) 
    {
        this.team = team;
    }

    public Group(Team team, List<TeamMember> members)
    {
        this.team = team;
        this.members = members;
    }

    public List<TeamMember> AddMember(TeamMember member) 
    {
        members.Add(member);
        return members;
    }
    
    public List<TeamMember> AddMembers(List<TeamMember> members)
    {
        members.AddRange(members);
        return members;
    }

}