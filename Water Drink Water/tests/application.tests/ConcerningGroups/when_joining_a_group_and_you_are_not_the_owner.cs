using Ardalis.Result;
using TbdFriends.WaterDrinkWater.Application.Contracts;

namespace application.tests.ConcerningGroups;

public class when_joining_a_group_and_you_are_not_the_owner
{
    [Fact]
    public void membership_record_is_created()
    {
// Arrange
        const int AccountId = 10001;
        const int GroupId = 20002;
        const string GroupCode = "GroupCode";

        var repository = Substitute.For<IGroupRepository>();
        var generator = Substitute.For<ICodeGenerator>();

        repository
            .GetByCode(GroupCode)
            .Returns(new Group()
            {
                Id = GroupId,
                OwnerId = 20002
            });

        var service = new GroupService(repository, generator);

        // Act

        var result = service.JoinGroup(GroupCode, AccountId);

        // Assert

        repository
            .Received()
            .AddMembership(Arg.Is<Membership>(m => m.GroupId == GroupId && m.AccountId == AccountId));

        Assert.True(result);
    }
}