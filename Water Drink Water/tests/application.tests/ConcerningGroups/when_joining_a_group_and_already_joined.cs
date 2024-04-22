using Ardalis.Result;
using TbdFriends.WaterDrinkWater.Application.Contracts;

namespace application.tests.ConcerningGroups;

public class when_joining_a_group_and_already_joined
{
    [Fact]
    public void membership_is_not_duplicated()
    {
        // Arrange
        const int UserId = 10001;
        const int GroupId = 20001;
        const string GroupCode = "GroupCode";

        var repository = Substitute.For<IGroupRepository>();
        var generator = Substitute.For<ICodeGenerator>();

        repository
            .GetByCode(GroupCode)
            .Returns(new Group()
            {
                Id = GroupId,
                OwnerId = 10002
            });

        repository
            .GetMembershipsForGroup(Arg.Is(GroupId))
            .Returns(new List<Membership>
            {
                new()
                {
                    Id = 1,
                    GroupId = GroupId,
                    AccountId = UserId
                }
            });

        var service = new GroupService(repository, generator);

        // Act

        var result = service.JoinGroup(GroupCode, UserId);

        // Assert

        repository
            .DidNotReceive()
            .AddMembership(Arg.Any<Membership>());

        Assert.False(result);
    }
}