using EFCore;
using EFCore.Models.S2;
using Microsoft.EntityFrameworkCore;

using var db = new AppDbContext();

// ---------- INSERT ----------
var organizer = new Organizer
{
    Name = "Omar",
    CompanyName = "Learnovia",
    IsVerified = true,
    Profile = new OrganizerProfile { Bio = "We run tech events", Logo = "logo.png", Link = "https://example.com" },
    Events =
    {
        new Event { Title = "EF Core Workshop", Description = "Hands-on EF Core", StartDate = DateTime.Today.AddDays(7), MaxAttendees = 50 },
        new Event { Title = "C# Meetup", Description = "Monthly meetup", StartDate = DateTime.Today.AddDays(14), EndDate = DateTime.Today.AddDays(15), MaxAttendees = 100 }
    }
};

var withBadge = new Attendee
{
    FullName = "Sara Ali",
    Email = "sara@example.com",
    Address = new Address { Street = "10 Nile St", City = "Cairo", Country = "Egypt", PostalCode = "11511" },
    Badge = new Badge { Number = 1001, IssuedOn = DateTime.Today, BadgeTier = BadgeTier.VIP }
};

var noBadge = new Attendee   // tests "at most one" → zero is allowed
{
    FullName = "Ahmed Hassan",
    Email = "ahmed@example.com",
    Address = new Address { Street = "5 Tahrir Sq", City = "Giza", Country = "Egypt", PostalCode = "12511" }
};

db.AddRange(organizer, withBadge, noBadge);
db.SaveChanges();   // EF inserts in the right order and fills in all the FKs for you

// ---------- READ BACK ----------
db.ChangeTracker.Clear();   // forget the tracked objects, so we really read from the DB

var org = db.Set<Organizer>()
    .Include(o => o.Profile)
    .Include(o => o.Events)
    .First();

Console.WriteLine($"{org.Name} ({org.CompanyName}) verified={org.IsVerified}");
Console.WriteLine($"  Profile: {org.Profile?.Bio}");
foreach (var e in org.Events)
    Console.WriteLine($"  Event: {e.Title} on {e.StartDate:d}");

foreach (var a in db.Set<Attendee>().Include(a => a.Badge))
    Console.WriteLine($"{a.FullName} | {a.FullAddress} | Badge: {(a.Badge is null ? "none" : $"#{a.Badge.Number} {a.Badge.BadgeTier}")}");
