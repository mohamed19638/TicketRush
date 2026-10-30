using TicketRush.Domain.Common;

namespace TicketRush.Domain.Catalog;

public sealed class Show
{
    private Show()
    {
    }

    private Show(
        Guid id,
        Guid eventId,
        Guid venueId,
        DateTime startsAtUtc,
        DateTime doorsOpenAtUtc,
        DateTime onSaleAtUtc,
        DateTime salesCloseAtUtc,
        CurrencyCode currency,
        DateTime nowUtc)
    {
        Id = id;
        EventId = eventId;
        VenueId = venueId;
        StartsAtUtc = startsAtUtc;
        DoorsOpenAtUtc = doorsOpenAtUtc;
        OnSaleAtUtc = onSaleAtUtc;
        SalesCloseAtUtc = salesCloseAtUtc;
        Currency = currency;
        Status = ShowStatus.Draft;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public Guid Id { get; private set; }

    public Guid EventId { get; private set; }

    public Guid VenueId { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime DoorsOpenAtUtc { get; private set; }

    public DateTime OnSaleAtUtc { get; private set; }

    public DateTime SalesCloseAtUtc { get; private set; }

    public ShowStatus Status { get; private set; }

    public CurrencyCode Currency { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public static Show Create(
        Guid id,
        Guid eventId,
        Guid venueId,
        DateTime startsAtUtc,
        DateTime doorsOpenAtUtc,
        DateTime onSaleAtUtc,
        DateTime salesCloseAtUtc,
        CurrencyCode currency,
        DateTime nowUtc)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireId(eventId, nameof(eventId));
        DomainGuard.RequireId(venueId, nameof(venueId));
        DomainGuard.RequireUtc(startsAtUtc, nameof(startsAtUtc));
        DomainGuard.RequireUtc(doorsOpenAtUtc, nameof(doorsOpenAtUtc));
        DomainGuard.RequireUtc(onSaleAtUtc, nameof(onSaleAtUtc));
        DomainGuard.RequireUtc(salesCloseAtUtc, nameof(salesCloseAtUtc));
        DomainGuard.RequireUtc(nowUtc, nameof(nowUtc));
        DomainGuard.RequireDefined(currency, nameof(currency));

        if (doorsOpenAtUtc > startsAtUtc)
        {
            throw new ArgumentException("Doors cannot open after the show starts.", nameof(doorsOpenAtUtc));
        }

        if (onSaleAtUtc >= salesCloseAtUtc)
        {
            throw new ArgumentException("Sales must close after they open.", nameof(salesCloseAtUtc));
        }

        if (salesCloseAtUtc > startsAtUtc)
        {
            throw new ArgumentException("Sales must close no later than the show start.", nameof(salesCloseAtUtc));
        }

        return new Show(id, eventId, venueId, startsAtUtc, doorsOpenAtUtc, onSaleAtUtc, salesCloseAtUtc, currency, nowUtc);
    }
}
