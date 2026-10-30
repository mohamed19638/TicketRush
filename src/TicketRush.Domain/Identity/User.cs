using TicketRush.Domain.Common;

namespace TicketRush.Domain.Identity;

public sealed class User
{
    private User()
    {
    }

    private User(Guid id, EmailAddress email, string displayName, PasswordHash passwordHash, DateTime nowUtc)
    {
        Id = id;
        Email = email;
        DisplayName = displayName;
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public Guid Id { get; private set; }

    public EmailAddress Email { get; private set; } = null!;

    public string DisplayName { get; private set; } = string.Empty;

    public PasswordHash PasswordHash { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public static User Create(Guid id, EmailAddress email, string displayName, PasswordHash passwordHash, DateTime nowUtc)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireUtc(nowUtc, nameof(nowUtc));

        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(passwordHash);

        return new User(id, email, DomainGuard.RequireText(displayName, nameof(displayName)), passwordHash, nowUtc);
    }
}
