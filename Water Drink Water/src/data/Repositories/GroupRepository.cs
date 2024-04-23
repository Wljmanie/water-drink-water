using Microsoft.EntityFrameworkCore;
using TbdFriends.WaterDrinkWater.Data.Contexts;
using TbdFriends.WaterDrinkWater.Data.Contracts;
using TbdFriends.WaterDrinkWater.Data.Models;

namespace TbdFriends.WaterDrinkWater.Data.Repositories;

public class GroupRepository(IDbContextFactory<ApplicationDbContext> factory) : IGroupRepository
{
    public void Add(Group group)
    {
        using var context = factory.CreateDbContext();

        context.Groups.Add(group);

        context.SaveChanges();
    }

    public Group? GetByName(string name, int accountId)
    {
        using var context = factory.CreateDbContext();

        return context.Groups.FirstOrDefault(g => g.Name == name && g.OwnerId == accountId);
    }

    public IEnumerable<Group> GetGroups(int accountId)
    {
        using var context = factory.CreateDbContext();

        return context.Groups
            .Where(g => g.OwnerId == accountId)
            .ToList();
    }

    public Group? GetByCode(string code)
    {
        using var context = factory.CreateDbContext();

        return context.Groups.FirstOrDefault(g => g.Code == code);
    }

    public void AddMembership(Membership membership)
    {
        using var context = factory.CreateDbContext();

        context.Memberships.Add(membership);

        context.SaveChanges();
    }

    public IEnumerable<Membership> GetMembershipsForGroup(int groupId)
    {
        using var context = factory.CreateDbContext();

        return context.Memberships
            .Where(m => m.GroupId == groupId)
            .ToList();
    }
}