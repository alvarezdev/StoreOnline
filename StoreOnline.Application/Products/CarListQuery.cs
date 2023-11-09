using StoreOnline.Domain.Dto;
using MediatR;

namespace StoreOnline.Application.Products;

public record CarListQuery : IRequest<IEnumerable<CarDto>>;
