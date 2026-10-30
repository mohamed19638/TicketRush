using TicketRush.Domain.Common;

namespace TicketRush.Domain.Catalog;

public sealed class PriceTier
{
    private PriceTier()
    {
    }

    private PriceTier(Guid id, Guid showId, string name, string code, long priceMinor, CurrencyCode currency, int salesLimit, int sortOrder)
    {
        Id = id;
        ShowId = showId;
        Name = name;
        Code = code;
        PriceMinor = priceMinor;
        Currency = currency;
        SalesLimit = salesLimit;
        SortOrder = sortOrder;
        IsActive = true;
    }

    public Guid Id { get; private set; }

    public Guid ShowId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Code { get; private set; } = string.Empty;

    public long PriceMinor { get; private set; }

    public CurrencyCode Currency { get; private set; }

    public int SalesLimit { get; private set; }

    public bool IsActive { get; private set; }

    public int SortOrder { get; private set; }

    public static PriceTier Create(
        Guid id,
        Guid showId,
        string name,
        string code,
        long priceMinor,
        CurrencyCode currency,
        CurrencyCode showCurrency,
        int salesLimit,
        int sortOrder)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireId(showId, nameof(showId));
        DomainGuard.RequireDefined(currency, nameof(currency));
        DomainGuard.RequireDefined(showCurrency, nameof(showCurrency));

        if (priceMinor < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priceMinor), "Price cannot be negative.");
        }

        if (salesLimit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(salesLimit), "Sales limit must be positive.");
        }

        if (sortOrder < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sortOrder), "Sort order cannot be negative.");
        }

        if (currency != showCurrency)
        {
            throw new ArgumentException("A price tier must use its show's currency.", nameof(currency));
        }

        return new PriceTier(
            id,
            showId,
            DomainGuard.RequireText(name, nameof(name)),
            DomainGuard.RequireText(code, nameof(code)).ToUpperInvariant(),
            priceMinor,
            currency,
            salesLimit,
            sortOrder);
    }
}
