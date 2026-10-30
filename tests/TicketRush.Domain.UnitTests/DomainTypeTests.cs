using TicketRush.Domain.Catalog;
using TicketRush.Domain.Identity;
using TicketRush.Domain.Venues;

namespace TicketRush.Domain.UnitTests;

public class DomainTypeTests
{
    private static readonly DateTime NowUtc = new(2026, 10, 31, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void EmailAddress_NormalizesCaseAndWhitespace()
    {
        var email = EmailAddress.Create("  Sam@example.com ");

        Assert.Equal("SAM@EXAMPLE.COM", email.NormalizedValue);
    }

    [Fact]
    public void EmailAddress_RejectsInvalidValue()
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Create("not-an-email"));
    }

    [Fact]
    public void User_RequiresUtcCreationTime()
    {
        var localTime = DateTime.SpecifyKind(NowUtc, DateTimeKind.Local);

        Assert.Throws<ArgumentException>(() => User.Create(
            Guid.NewGuid(),
            EmailAddress.Create("sam@example.com"),
            "Sam",
            PasswordHash.FromEncodedValue("hash"),
            localTime));
    }

    [Fact]
    public void RefreshToken_RequiresExpiryAfterCreation()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RefreshToken.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            RefreshTokenHash.FromEncodedValue("hash"),
            NowUtc,
            NowUtc));
    }

    [Fact]
    public void Section_RequiresPositiveCapacity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Section.Create(
            Guid.NewGuid(), Guid.NewGuid(), "A", "Floor", 0, 0, SectionType.Seated));
    }

    [Fact]
    public void Seat_RequiresPositiveSeatNumber()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Seat.Create(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "A", 0, "A-0", false));
    }

    [Fact]
    public void Event_NormalizesSlugAndStartsAsDraft()
    {
        var ticketEvent = Event.Create(
            Guid.NewGuid(), Guid.NewGuid(), "Concert", "  Summer-Show  ", null, "Music", NowUtc);

        Assert.Equal("summer-show", ticketEvent.Slug);
        Assert.Equal(EventStatus.Draft, ticketEvent.Status);
    }

    [Fact]
    public void Event_RejectsSlugWithUnsupportedCharacters()
    {
        Assert.Throws<ArgumentException>(() => Event.Create(
            Guid.NewGuid(), Guid.NewGuid(), "Concert", "summer show", null, "Music", NowUtc));
    }

    [Fact]
    public void Show_RequiresUtcInstants()
    {
        var localTime = DateTime.SpecifyKind(NowUtc, DateTimeKind.Local);

        Assert.Throws<ArgumentException>(() => Show.Create(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            NowUtc.AddDays(1), NowUtc.AddDays(1).AddHours(-1), NowUtc,
            NowUtc.AddHours(1), CurrencyCode.EGP, localTime));
    }

    [Fact]
    public void Show_RequiresSalesToCloseBeforeStart()
    {
        var startsAt = NowUtc.AddDays(1);

        Assert.Throws<ArgumentException>(() => Show.Create(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            startsAt, startsAt.AddHours(-1), NowUtc, startsAt.AddMinutes(1), CurrencyCode.EGP, NowUtc));
    }

    [Fact]
    public void PriceTier_RequiresNonNegativePriceAndMatchingCurrency()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PriceTier.Create(
            Guid.NewGuid(), Guid.NewGuid(), "Standard", "STD", -1,
            CurrencyCode.EGP, CurrencyCode.EGP, 100, 0));

        Assert.Throws<ArgumentException>(() => PriceTier.Create(
            Guid.NewGuid(), Guid.NewGuid(), "Standard", "STD", 1000,
            CurrencyCode.USD, CurrencyCode.EGP, 100, 0));
    }

    [Fact]
    public void PriceTier_NormalizesCode()
    {
        var tier = PriceTier.Create(
            Guid.NewGuid(), Guid.NewGuid(), "Standard", " std ", 2500,
            CurrencyCode.EGP, CurrencyCode.EGP, 100, 0);

        Assert.Equal("STD", tier.Code);
        Assert.Equal(2500, tier.PriceMinor);
    }
}
