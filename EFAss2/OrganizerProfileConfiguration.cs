using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EventHub.Models;

namespace EventHub.Configurations;

public class OrganizerProfileConfiguration : IEntityTypeConfiguration<OrganizerProfile>
{
    public void Configure(EntityTypeBuilder<OrganizerProfile> builder)
    {
        builder.ToTable("OrganizerProfiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Biography).HasMaxLength(1000);
        builder.Property(p => p.WebsiteUrl).HasMaxLength(300);
        builder.Property(p => p.LogoPath).HasMaxLength(500);

        builder.HasOne(p => p.Organizer)
               .WithOne(o => o.Profile)
               .HasForeignKey<OrganizerProfile>(p => p.OrganizerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
