using StoreOnline.Application.Products;
using MediatR;
using StoreOnline.Domain.Dto;

namespace StoreOnline.Api.ApiHandlers;

public static class ProductApi
{
    public static RouteGroupBuilder MapCar(this IEndpointRouteBuilder routeHandler)
    {
        routeHandler.MapPost("/product", async (IMediator mediator, CarRegisterCommand productCommand) =>
        {
            var product = await mediator.Send(productCommand);
            return Results.Created(new Uri($"/api/product/", UriKind.Relative), product);
        })
        .Produces(statusCode: StatusCodes.Status201Created);

        routeHandler.MapGet("/list", async (IMediator mediator) =>
        {
            return Results.Ok(await mediator.Send(new CarListQuery()));
        })
        .Produces(StatusCodes.Status200OK, typeof(IEnumerable<CarDto>));

        routeHandler.MapGet("/listSold", async (IMediator mediator) =>
        {
            return Results.Ok(await mediator.Send(new CarListSoldQuery()));
        })
        .Produces(StatusCodes.Status200OK, typeof(IEnumerable<CarDto>));       

        return (RouteGroupBuilder)routeHandler;
    }
}

