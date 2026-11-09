namespace EventHub.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MaxAttendees { get; set; }

    public int OrganizerId { get; set; }
    public Organizer Organizer { get; set; } = null!;
}
