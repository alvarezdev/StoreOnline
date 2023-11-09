using StoreOnline.Application.Products;
using MediatR;
using StoreOnline.Domain.Dto;

namespace StoreOnline.Api.ApiHandlers;

public static class BuyApi
{
    public static RouteGroupBuilder MapBuy(this IEndpointRouteBuilder routeHandler)
    {
        routeHandler.MapPost("/buy", async (IMediator mediator, BuyRegisterCommand buyCommand) =>
        {
            var buy = await mediator.Send(buyCommand);
            return Results.Created(new Uri($"/api/buy/", UriKind.Relative), buy);
        })
        .Produces(statusCode: StatusCodes.Status201Created);    

        return (RouteGroupBuilder)routeHandler;
    }
}

