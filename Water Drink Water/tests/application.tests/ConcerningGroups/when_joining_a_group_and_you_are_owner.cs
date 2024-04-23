using Ardalis.Result;
using TbdFriends.WaterDrinkWater.Application.Contracts;

namespace application.tests.ConcerningGroups;

public class when_joining_a_group_and_you_are_owner
{
    [Fact]
    public void no_membership_record_is_created()
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
                OwnerId = UserId
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