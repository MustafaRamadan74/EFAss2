using EventHub.Data;
using EventHub.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace EFAss2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            using var context = new EventHubContext();

            Console.WriteLine($"Database reachable: {context.Database.CanConnect()}");

            if (!context.Organizers.Any())
            {
                var nour = new Organizer
                {
                    Name = "Nour Salem",
                    CompanyName = "Nour Events LLC",
                    IsVerified = true,
                    Profile = new OrganizerProfile
                    {
                        Biography = "We run tech conferences across Egypt.",
                        WebsiteUrl = "https://nour-events.example.com",
                        LogoPath = "logos/nour-events.png"
                    }
                };

                var hassan = new Organizer
                {
                    Name = "Hassan Ali",
                    IsVerified = false,
                    Profile = new OrganizerProfile
                    {
                        Biography = "Independent photographer and workshop host."
                    }
                };

                context.Events.AddRange(
                    new Event { Title = "Cairo Tech Summit", Description = "Two days of talks and workshops on software and data.", StartDate = new DateTime(2026, 12, 10, 9, 0, 0), EndDate = new DateTime(2026, 12, 11, 17, 0, 0), MaxAttendees = 500, Organizer = nour },
                    new Event { Title = "Photography Workshop", Description = "Hands-on street photography workshop.", StartDate = new DateTime(2027, 1, 15, 10, 0, 0), EndDate = null, MaxAttendees = 30, Organizer = hassan }
                );

                context.Attendees.AddRange(
                    new Attendee
                    {
                        FullName = "Mona Samir",
                        Email = "mona.samir@example.com",
                        Street = "12 Tahrir St",
                        City = "Cairo",
                        Country = "Egypt",
                        PostalCode = "11511",
                        Badge = new Badge { BadgeNumber = "BDG-0001", IssuedDate = new DateTime(2026, 11, 9), Tier = BadgeTier.Standard }
                    },
                    new Attendee
                    {
                        FullName = "Karim Adel",
                        Email = "karim.adel@example.com",
                        Street = "5 Nile Corniche",
                        City = "Giza",
                        Country = "Egypt",
                        PostalCode = "12511",
                        Badge = new Badge { BadgeNumber = "BDG-0002", IssuedDate = new DateTime(2026, 11, 9), Tier = BadgeTier.VIP }
                    },
                    new Attendee
                    {
                        FullName = "Laila Hany",
                        Email = "laila.hany@example.com",
                        Street = "20 Palm Hills Rd",
                        City = "Alexandria",
                        Country = "Egypt",
                        PostalCode = "21500"
                    }
                );

                context.SaveChanges();
            }

            Console.WriteLine();
            Console.WriteLine("Organizers:");
            var organizers = context.Organizers.Include(o => o.Profile).Include(o => o.Events).OrderBy(o => o.Id).ToList();
            foreach (var o in organizers)
            {
                Console.WriteLine($"{o.Id,-3}{o.Name,-14}{o.CompanyName ?? "(individual)",-18}Verified: {o.IsVerified,-6}Events: {o.Events.Count}  Bio: {o.Profile?.Biography}");
            }

            Console.WriteLine();
            Console.WriteLine("Events:");
            var events = context.Events.Include(e => e.Organizer).OrderBy(e => e.Id).ToList();
            foreach (var e in events)
            {
                Console.WriteLine($"{e.Id,-3}{e.Title,-24}{e.StartDate:yyyy-MM-dd}  End: {(e.EndDate.HasValue ? e.EndDate.Value.ToString("yyyy-MM-dd") : "-"),-11}Max: {e.MaxAttendees,-5}By: {e.Organizer.Name}");
            }

            Console.WriteLine();
            Console.WriteLine("Attendees:");
            var attendees = context.Attendees.Include(a => a.Badge).OrderBy(a => a.Id).ToList();
            foreach (var a in attendees)
            {
                var badge = a.Badge is null ? "no badge" : $"{a.Badge.BadgeNumber} ({a.Badge.Tier})";
                Console.WriteLine($"{a.Id,-3}{a.FullName,-13}{badge,-20}{a.FullAddress}");
            }
        }
    }
}
