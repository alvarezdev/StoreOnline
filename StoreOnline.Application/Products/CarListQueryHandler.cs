using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Ports;
using MediatR;
using StoreOnline.Application.Anticorruption;

namespace StoreOnline.Application.Products;

public class CarListQueryHandler : IRequestHandler<CarListQuery, IEnumerable<CarDto>>
{
    private readonly ICarRepository _repository;
    public CarListQueryHandler(ICarRepository repository) => _repository = repository;
    

    public async Task<IEnumerable<CarDto>> Handle(CarListQuery request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetListAsync();
        return CarFactory.FromCarListModelToDto(list);
    }
}
