using TicketRush.Domain.Common;

namespace TicketRush.Domain.Catalog;

public sealed class Event
{
    private Event()
    {
    }

    private Event(
        Guid id,
        Guid organizerId,
        string title,
        string slug,
        string? description,
        string category,
        DateTime nowUtc)
    {
        Id = id;
        OrganizerId = organizerId;
        Title = title;
        Slug = slug;
        Description = description;
        Category = category;
        Status = EventStatus.Draft;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public Guid Id { get; private set; }

    public Guid OrganizerId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string Category { get; private set; } = string.Empty;

    public EventStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public DateTime? PublishedAtUtc { get; private set; }

    public DateTime? CancelledAtUtc { get; private set; }

    public static Event Create(
        Guid id,
        Guid organizerId,
        string title,
        string slug,
        string? description,
        string category,
        DateTime nowUtc)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireId(organizerId, nameof(organizerId));
        DomainGuard.RequireUtc(nowUtc, nameof(nowUtc));

        var normalizedSlug = NormalizeSlug(slug);
        var cleanDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

        return new Event(
            id,
            organizerId,
            DomainGuard.RequireText(title, nameof(title)),
            normalizedSlug,
            cleanDescription,
            DomainGuard.RequireText(category, nameof(category)),
            nowUtc);
    }

    private static string NormalizeSlug(string value)
    {
        var slug = DomainGuard.RequireText(value, nameof(value)).ToLowerInvariant();

        if (slug[0] == '-' || slug[^1] == '-' || slug.Contains("--", StringComparison.Ordinal))
        {
            throw new ArgumentException("The slug cannot start, end, or repeat a hyphen.", nameof(value));
        }

        if (slug.Any(character => !(character is >= 'a' and <= 'z' or >= '0' and <= '9' or '-')))
        {
            throw new ArgumentException("The slug may contain only letters, numbers, and hyphens.", nameof(value));
        }

        return slug;
    }
}
