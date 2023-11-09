using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Ports;
using MediatR;
using StoreOnline.Application.Anticorruption;

namespace StoreOnline.Application.Products;

public class CarListSoldQueryHandler : IRequestHandler<CarListSoldQuery, IEnumerable<CarDto>>
{
    private readonly ICarRepository _repository;
    public CarListSoldQueryHandler(ICarRepository repository) => _repository = repository;
    

    public async Task<IEnumerable<CarDto>> Handle(CarListSoldQuery request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetListAsync();
        var listReturn = CarFactory.FromCarListModelToDto(list);      
        return listReturn.Where(x => x.IsSold); 
    }
}
