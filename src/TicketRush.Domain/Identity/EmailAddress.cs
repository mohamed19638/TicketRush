using System.Net.Mail;

namespace TicketRush.Domain.Identity;

public sealed record EmailAddress
{
    private EmailAddress(string normalizedValue)
    {
        NormalizedValue = normalizedValue;
    }

    public string NormalizedValue { get; }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("An email address is required.", nameof(value));
        }

        var normalized = value.Trim().ToUpperInvariant();

        if (!MailAddress.TryCreate(normalized, out var parsed)
            || !string.Equals(parsed.Address, normalized, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The email address is not valid.", nameof(value));
        }

        return new EmailAddress(normalized);
    }
}

public sealed record PasswordHash
{
    private PasswordHash(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static PasswordHash FromEncodedValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("A password hash is required.", nameof(value));
        }

        return new PasswordHash(value.Trim());
    }
}
