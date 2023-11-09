using StoreOnline.Domain.Dto;
using MediatR;

namespace StoreOnline.Application.Products;

public record CarListSoldQuery : IRequest<IEnumerable<CarDto>>;
