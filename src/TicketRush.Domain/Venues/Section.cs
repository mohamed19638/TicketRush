using TicketRush.Domain.Common;

namespace TicketRush.Domain.Venues;

public sealed class Section
{
    private Section()
    {
    }

    private Section(Guid id, Guid venueId, string code, string name, int sortOrder, int capacity, SectionType type)
    {
        Id = id;
        VenueId = venueId;
        Code = code;
        Name = name;
        SortOrder = sortOrder;
        Capacity = capacity;
        Type = type;
    }

    public Guid Id { get; private set; }

    public Guid VenueId { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public int SortOrder { get; private set; }

    public int Capacity { get; private set; }

    public SectionType Type { get; private set; }

    public static Section Create(Guid id, Guid venueId, string code, string name, int sortOrder, int capacity, SectionType type)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireId(venueId, nameof(venueId));
        DomainGuard.RequireDefined(type, nameof(type));

        if (sortOrder < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sortOrder), "Sort order cannot be negative.");
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be positive.");
        }

        return new Section(id, venueId, DomainGuard.RequireText(code, nameof(code)).ToUpperInvariant(), DomainGuard.RequireText(name, nameof(name)), sortOrder, capacity, type);
    }
}
