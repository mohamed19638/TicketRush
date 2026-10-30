using TicketRush.Domain.Common;

namespace TicketRush.Domain.Identity;

public sealed class Role
{
    private Role()
    {
    }

    private Role(Guid id, RoleName name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }

    public RoleName Name { get; private set; }

    public static Role Create(Guid id, RoleName name)
    {
        DomainGuard.RequireId(id, nameof(id));
        DomainGuard.RequireDefined(name, nameof(name));

        return new Role(id, name);
    }
}
