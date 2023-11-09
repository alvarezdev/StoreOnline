using StoreOnline.Domain.Ports;
using StoreOnline.Domain.Services;
using NSubstitute;
using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Tests.Builders;
using StoreOnline.Domain.Exceptions;
using NuGet.Frameworks;

namespace StoreOnline.Domain.Tests;

public class RegisterBuyTests
{   
    readonly ICarRepository _productRepository = default!;
    readonly IBuyRepository _buyRepository = default!;
    readonly IUnitOfWork _unitOfWork;
    readonly RegisterBuyService _service = default!;

    public RegisterBuyTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _productRepository = Substitute.For<ICarRepository>();
        _buyRepository = Substitute.For<IBuyRepository>();
        _service = new RegisterBuyService(_productRepository, _buyRepository, _unitOfWork);
    }

    [Fact]
    public void CreateBuy_WithCarIdEmpty_ThrowsCoreBusinessException()
    {
        try
        {
            BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.Empty)
                            .SetPrice(50000000)
                            .Build();
            Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest); 
            Assert.Fail("Should not get here");
        }
        catch (CoreBusinessException ex)
        {
            Assert.True(ex.Message.Equals("The car id must not be empty"));
        }
    }

    [Fact]
    public void CreateBuy_WithZeroBuyPrice_ThrowsCoreBusinessException()
    {
        try
        {
            BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetPrice(0)
                            .Build();
            Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest); 
            Assert.Fail("Should not get here");
        }
        catch (CoreBusinessException ex)
        {
            Assert.True(ex.Message.Equals("The price must be greater than 0"));
        }
    }

    [Fact]
    public void CreateBuy_WhenBuyMadeWithCreditCard_ShouldCreateBuy()
    {
        //Arrange
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.NewGuid())
                            .SetPaymentMethod(Enums.PaymentMethod.CreditCar)
                            .SetPrice(100000000)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 1);

        //Assert
        Assert.True(buy.Price == 105000000);
    }

    [Fact]
    public void CreateBuy_WhenBuyMadeWithCreditCardAndDay15_ShouldCreateBuy()
    {
        //Arrange
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.NewGuid())
                            .SetPaymentMethod(Enums.PaymentMethod.CreditCar)
                            .SetPrice(100000000)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 15);

        //Assert
        Assert.True(buy.Price == 95000000);
    }

    [Fact]
    public void CreateBuy_WhenBuyMadeWithCreditCardAndDay10_ShouldCreateBuy()
    {
        //Arrange
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.NewGuid())
                            .SetPaymentMethod(Enums.PaymentMethod.CreditCar)
                            .SetPrice(100000000)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 10);

        //Assert
        Assert.True(buy.Price == 95000000);
    }

    [Fact]
    public void CreateBuy_WhenBuyMadeWithDebitCard_ShouldCreateBuy()
    {
        //Arrange
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.NewGuid())
                            .SetPrice(100000000)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 1);                

        //Assert
        Assert.True(buy.Price == 100000000);
    }

    [Fact]
    public void CreateBuy_WhenBuyMadeWithDebitCardAndDay10_ShouldCreateBuy()
    {
        //Arrange
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.NewGuid())
                            .SetPrice(100000000)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 10);                

        //Assert
        Assert.True(buy.Price == 90000000);
    }

    [Fact]
    public void CreateBuy_WhenBuyMadeWithDebitCardAndDay15_ShouldCreateBuy()
    {
        //Arrange
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.NewGuid())
                            .SetPrice(100000000)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 15);                

        //Assert
        Assert.True(buy.Price == 90000000);
    }

    [Fact]
    public async void RegisterBuyAsync_WhenBuyMadeWithCreditCard_ShouldRegisterBuy()
    {
        //Arrange
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                        .SetPrice(car.Price)
                        .SetPaymentMethod(Enums.PaymentMethod.CreditCar)
                        .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 1); 
        _productRepository.GetAsync(Arg.Any<Guid>()).Returns(car);
        _productRepository.UpdateAsync(Arg.Any<Car>()).Returns(car);
        _buyRepository.SaveAsync(Arg.Any<Buy>()).Returns(buy);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        var result = await _service.RegisterBuyAsync(buy, new CancellationTokenSource().Token);
        await _productRepository.Received().GetAsync(Arg.Any<Guid>());
        await _productRepository.Received().UpdateAsync(Arg.Any<Car>());
        await _buyRepository.Received().SaveAsync(Arg.Any<Buy>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.True(buy.Price == 105000000);
        Assert.True(result.IsSold);
    }  

    [Fact]
    public async void RegisterBuyAsync_WhenBuyMadeWithDebitCard_ShouldRegisterBuy()
    {
        //Arrange
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetPrice(car.Price)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 1);  
        _productRepository.GetAsync(Arg.Any<Guid>()).Returns(car);
        _productRepository.UpdateAsync(Arg.Any<Car>()).Returns(car);
        _buyRepository.SaveAsync(Arg.Any<Buy>()).Returns(buy);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        var result = await _service.RegisterBuyAsync(buy, new CancellationTokenSource().Token);
        await _productRepository.Received().GetAsync(Arg.Any<Guid>());
        await _productRepository.Received().UpdateAsync(Arg.Any<Car>());
        await _buyRepository.Received().SaveAsync(Arg.Any<Buy>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.True(buy.Price == 100000000);
        Assert.True(result.IsSold);
    }

    [Fact]
    public async void RegisterBuyAsync_WhenBuyMadeWithCreditCardAndTokenNull_ShouldRegisterBuy()
    {
        //Arrange
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);
        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                        .SetPrice(car.Price)
                        .SetPaymentMethod(Enums.PaymentMethod.CreditCar)
                        .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 1);  
        _productRepository.GetAsync(Arg.Any<Guid>()).Returns(car);
        _productRepository.UpdateAsync(Arg.Any<Car>()).Returns(car);
        _buyRepository.SaveAsync(Arg.Any<Buy>()).Returns(buy);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        var result = await _service.RegisterBuyAsync(buy, null);
        await _productRepository.Received().GetAsync(Arg.Any<Guid>());
        await _productRepository.Received().UpdateAsync(Arg.Any<Car>());
        await _buyRepository.Received().SaveAsync(Arg.Any<Buy>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.True(buy.Price == 105000000);
        Assert.True(result.IsSold);
    }  

    [Fact]
    public async void RegisterBuyAsync_WhenBuyMadeWithDebitCardAndTokenNull_ShouldRegisterBuy()
    {
        //Arrange
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);

        BuyDtoTest buyDtoTest = new BuyDtoTest.Builder()
                            .SetProductId(Guid.NewGuid())
                            .SetPrice(car.Price)
                            .Build();
        Buy buy = BuyTestFactory.FromBuyDtoTestToModel(buyDtoTest);
        buy.CalculatePrice(buy!.Price, 1);

        _productRepository.GetAsync(Arg.Any<Guid>()).Returns(car);
        _productRepository.UpdateAsync(Arg.Any<Car>()).Returns(car);
        _buyRepository.SaveAsync(Arg.Any<Buy>()).Returns(buy);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        var result = await _service.RegisterBuyAsync(buy, null);
        await _productRepository.Received().GetAsync(Arg.Any<Guid>());
        await _productRepository.Received().UpdateAsync(Arg.Any<Car>());
        await _buyRepository.Received().SaveAsync(Arg.Any<Buy>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.True(buy.Price == 100000000);       
        Assert.True(result.IsSold);
    }

}
