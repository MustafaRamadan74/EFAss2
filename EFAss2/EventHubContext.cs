using Microsoft.EntityFrameworkCore;
using EventHub.Configurations;
using EventHub.Models;

namespace EventHub.Data;

public class EventHubContext : DbContext
{
    public DbSet<Organizer> Organizers { get; set; } = null!;
    public DbSet<OrganizerProfile> OrganizerProfiles { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Attendee> Attendees { get; set; } = null!;
    public DbSet<Badge> Badges { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=.;Database=EventHubDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrganizerProfileConfiguration());
        modelBuilder.ApplyConfiguration(new BadgeConfiguration());

        modelBuilder.Entity<Event>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).IsRequired();
            e.Property(x => x.StartDate).IsRequired();
            e.Property(x => x.EndDate);
            e.Property(x => x.MaxAttendees).IsRequired();

            e.HasOne(x => x.Organizer)
             .WithMany(o => o.Events)
             .HasForeignKey(x => x.OrganizerId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
