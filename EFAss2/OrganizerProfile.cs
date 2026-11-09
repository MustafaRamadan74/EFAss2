namespace EventHub.Models;

public class OrganizerProfile
{
    public int Id { get; set; }
    public string? Biography { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LogoPath { get; set; }

    public int OrganizerId { get; set; }
    public Organizer Organizer { get; set; } = null!;
}
