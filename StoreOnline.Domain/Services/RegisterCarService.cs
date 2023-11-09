using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Exceptions;
using StoreOnline.Domain.Ports;

namespace StoreOnline.Domain.Services;

[DomainService]
public class RegisterCarService
{
    const string IS_SOLD_EXCEPTION_MESSAGE = "The car must be registered as unsold";
    private readonly ICarRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCarService(ICarRepository productRepository, IUnitOfWork unitOfWork) =>
        (_productRepository, _unitOfWork) = (productRepository, unitOfWork);
    
    public async Task<Car> RegisterCarAsync(Car car, CancellationToken? cancellationToken)
    {
        if (cancellationToken == null)
        {
            cancellationToken = new CancellationTokenSource().Token;
        }
        CheckIsSold(car!.IsSold);
        var carSaved = await _productRepository.SaveAsync(car);
        await _unitOfWork.SaveAsync(cancellationToken);
        return carSaved;   
    } 

    private static void CheckIsSold(bool isSold)
    {
        if(isSold)
        {
            throw new CoreBusinessException(IS_SOLD_EXCEPTION_MESSAGE);
        }        
    }

}
