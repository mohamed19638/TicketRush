using TicketRush.Domain.Common;

namespace TicketRush.Domain.Venues;

public sealed class Venue
{
    private Venue()
    {
    }

    private Venue(Guid id, Guid organizerId, string name, string city, string addressText, string timeZoneId, DateTime nowUtc)
    {
        Id = id;
        OrganizerId = organizerId;
        Name = name;
        City = city;
        AddressText = addressText;
        TimeZoneId = timeZoneId;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public Guid Id { get; private set; }

    public Guid OrganizerId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string City { get; private set; } = string.Empty;

    public string AddressText { get; private set; } = string.Empty;

    public string TimeZoneId { get; private set; } = string.Empty;

    public bool IsArchived { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public static Venue Create(
        Guid id,
        Guid organizerId,
        string name,
        string city,
        string addressText,
        string timeZoneId,
        DateTime nowUtc)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireId(organizerId, nameof(organizerId));
        DomainGuard.RequireUtc(nowUtc, nameof(nowUtc));

        return new Venue(
            id,
            organizerId,
            DomainGuard.RequireText(name, nameof(name)),
            DomainGuard.RequireText(city, nameof(city)),
            DomainGuard.RequireText(addressText, nameof(addressText)),
            DomainGuard.RequireText(timeZoneId, nameof(timeZoneId)),
            nowUtc);
    }
}
