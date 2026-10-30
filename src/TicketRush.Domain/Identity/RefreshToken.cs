using TicketRush.Domain.Common;

namespace TicketRush.Domain.Identity;

public sealed record RefreshTokenHash
{
    private RefreshTokenHash(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static RefreshTokenHash FromEncodedValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("A refresh-token hash is required.", nameof(value));
        }

        return new RefreshTokenHash(value.Trim());
    }
}

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    private RefreshToken(
        Guid id,
        Guid userId,
        Guid familyId,
        RefreshTokenHash tokenHash,
        DateTime createdAtUtc,
        DateTime expiresAtUtc,
        string? createdByIpHash)
    {
        Id = id;
        UserId = userId;
        FamilyId = familyId;
        TokenHash = tokenHash;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        CreatedByIpHash = createdByIpHash;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid FamilyId { get; private set; }

    public RefreshTokenHash TokenHash { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime ExpiresAtUtc { get; private set; }

    public DateTime? RevokedAtUtc { get; private set; }

    public Guid? ReplacedByTokenId { get; private set; }

    public DateTime? ReuseDetectedAtUtc { get; private set; }

    public string? CreatedByIpHash { get; private set; }

    public static RefreshToken Create(
        Guid id,
        Guid userId,
        Guid familyId,
        RefreshTokenHash tokenHash,
        DateTime createdAtUtc,
        DateTime expiresAtUtc,
        string? createdByIpHash = null)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireId(userId, nameof(userId));
        DomainGuard.RequireId(familyId, nameof(familyId));
        DomainGuard.RequireUtc(createdAtUtc, nameof(createdAtUtc));
        DomainGuard.RequireUtc(expiresAtUtc, nameof(expiresAtUtc));
        ArgumentNullException.ThrowIfNull(tokenHash);

        if (expiresAtUtc <= createdAtUtc)
        {
            throw new ArgumentOutOfRangeException(nameof(expiresAtUtc), "Expiry must be later than creation.");
        }

        var ipHash = string.IsNullOrWhiteSpace(createdByIpHash) ? null : createdByIpHash.Trim();

        return new RefreshToken(id, userId, familyId, tokenHash, createdAtUtc, expiresAtUtc, ipHash);
    }
}
