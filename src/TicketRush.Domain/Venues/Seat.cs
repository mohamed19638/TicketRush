using TicketRush.Domain.Common;

namespace TicketRush.Domain.Venues;

public sealed class Seat
{
    private Seat()
    {
    }

    private Seat(Guid id, Guid venueId, Guid sectionId, string rowLabel, int seatNumber, string displayLabel, bool isAccessible)
    {
        Id = id;
        VenueId = venueId;
        SectionId = sectionId;
        RowLabel = rowLabel;
        SeatNumber = seatNumber;
        DisplayLabel = displayLabel;
        IsAccessible = isAccessible;
        IsActive = true;
    }

    public Guid Id { get; private set; }

    public Guid VenueId { get; private set; }

    public Guid SectionId { get; private set; }

    public string RowLabel { get; private set; } = string.Empty;

    public int SeatNumber { get; private set; }

    public string DisplayLabel { get; private set; } = string.Empty;

    public bool IsAccessible { get; private set; }

    public bool IsActive { get; private set; }

    public static Seat Create(
        Guid id,
        Guid venueId,
        Guid sectionId,
        string rowLabel,
        int seatNumber,
        string displayLabel,
        bool isAccessible)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireId(venueId, nameof(venueId));
        DomainGuard.RequireId(sectionId, nameof(sectionId));

        if (seatNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(seatNumber), "Seat number must be positive.");
        }

        return new Seat(
            id,
            venueId,
            sectionId,
            DomainGuard.RequireText(rowLabel, nameof(rowLabel)),
            seatNumber,
            DomainGuard.RequireText(displayLabel, nameof(displayLabel)),
            isAccessible);
    }
}
