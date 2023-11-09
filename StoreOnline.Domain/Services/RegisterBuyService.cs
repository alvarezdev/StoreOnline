using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Enums;
using StoreOnline.Domain.Exceptions;
using StoreOnline.Domain.Ports;

namespace StoreOnline.Domain.Services;

[DomainService]
public class RegisterBuyService
{   
    private readonly ICarRepository _productRepository;
    private readonly IBuyRepository _buyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterBuyService(ICarRepository productRepository, IBuyRepository buyRepository, IUnitOfWork unitOfWork) =>
                            (_productRepository, _buyRepository, _unitOfWork) = (productRepository, buyRepository, unitOfWork);

    public async Task<Car> RegisterBuyAsync(Buy buy, CancellationToken? cancellationToken)
    {
        if (cancellationToken == null)
        {
            cancellationToken = new CancellationTokenSource().Token;
        }
        var car = await _productRepository.GetAsync(buy.ProductId);     
        car.IsSold = true;     
        var carUpdate = await _productRepository.UpdateAsync(car);        
        await _buyRepository.SaveAsync(buy!);
        await _unitOfWork.SaveAsync(cancellationToken);
        return carUpdate;
    }  
}
