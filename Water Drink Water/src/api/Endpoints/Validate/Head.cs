using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdFriends.WaterDrinkWater.Api.Endpoints.Validate;

public class Head : EndpointWithoutRequest<Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Head("api/validate");

        AllowAnonymous();
    }

    public override Task<Results<Ok, BadRequest>> ExecuteAsync(CancellationToken ct)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Task.FromResult<Results<Ok, BadRequest>>(TypedResults.BadRequest());
        }

        return Task.FromResult<Results<Ok, BadRequest>>(TypedResults.Ok());
    }
}