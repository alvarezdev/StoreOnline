using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Services;
using MediatR;
using StoreOnline.Application.Anticorruption;

namespace StoreOnline.Application.Products;

public class BuyRegisterCommandHandler : IRequestHandler<BuyRegisterCommand, BuyDto>
{
    private readonly RegisterBuyService _service;

    public BuyRegisterCommandHandler(RegisterBuyService service) =>
        _service = service ?? throw new ArgumentNullException(nameof(service));


   public async Task<BuyDto> Handle(BuyRegisterCommand request, CancellationToken cancellationToken)
    {  
        Buy buy = BuyFactory.FromBuyCommandToModel(request);

        var carSaved = await _service.RegisterBuyAsync(buy, cancellationToken);

        var buyDto = new BuyDto(buy!.ProductId, carSaved!.Price, buy!.PaymentMethod);

        return buyDto;
    }
}
