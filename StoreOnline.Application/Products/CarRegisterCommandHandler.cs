using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Services;
using MediatR;
using StoreOnline.Application.Anticorruption;

namespace StoreOnline.Application.Products;

public class CarRegisterCommandHandler : IRequestHandler<CarRegisterCommand, CarDto>
{
    private readonly RegisterCarService _service;

    public CarRegisterCommandHandler(RegisterCarService service) =>
        _service = service ?? throw new ArgumentNullException(nameof(service));


   public async Task<CarDto> Handle(CarRegisterCommand request, CancellationToken cancellationToken)
    {
        var car = CarFactory.FromCarCommandToModel(request);

        var productSaved = await _service.RegisterCarAsync(car, cancellationToken);

        return CarFactory.FromCarModelToDto(productSaved);
    }
}
