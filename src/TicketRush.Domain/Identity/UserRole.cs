using TicketRush.Domain.Common;

namespace TicketRush.Domain.Identity;

public sealed class UserRole
{
    private UserRole()
    {
    }

    private UserRole(Guid userId, Guid roleId, DateTime assignedAtUtc)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedAtUtc = assignedAtUtc;
    }

    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public DateTime AssignedAtUtc { get; private set; }

    public static UserRole Create(Guid userId, Guid roleId, DateTime assignedAtUtc)
    {
        DomainGuard.RequireId(userId, nameof(userId));
        DomainGuard.RequireId(roleId, nameof(roleId));
        DomainGuard.RequireUtc(assignedAtUtc, nameof(assignedAtUtc));

        return new UserRole(userId, roleId, assignedAtUtc);
    }
}
