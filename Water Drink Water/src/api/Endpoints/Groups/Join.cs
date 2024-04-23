using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using TbdFriends.WaterDrinkWater.Api.Infrastructure;
using TbdFriends.WaterDrinkWater.Application.Services;

namespace TbdFriends.WaterDrinkWater.Api.Endpoints.Groups;

public class Join(GroupService service) : EndpointWithoutRequest<Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("api/groups/{GroupCode}/join");
    }

    public override Task<Results<Ok, BadRequest>> ExecuteAsync(CancellationToken ct)
    {
        if (User.GetUserId() is not (var userId and > 0))
        {
            return Task.FromResult<Results<Ok, BadRequest>>(TypedResults.BadRequest());
        }

        var result = service.JoinGroup(Route<string>("GroupCode")!, userId);

        return Task.FromResult<Results<Ok, BadRequest>>(result
            ? TypedResults.Ok()
            : TypedResults.BadRequest());
    }
}