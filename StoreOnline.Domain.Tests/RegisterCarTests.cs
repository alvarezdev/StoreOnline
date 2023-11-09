using StoreOnline.Domain.Ports;
using StoreOnline.Domain.Services;
using NSubstitute;
using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Exceptions;
using StoreOnline.Domain.Tests.Builders;

namespace StoreOnline.Domain.Tests;

public class RegisterCarTests
{   
    readonly ICarRepository _repository = default!;
    readonly IUnitOfWork _unitOfWork;
    readonly RegisterCarService _service = default!;

    public RegisterCarTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _repository = Substitute.For<ICarRepository>();
        _service = new RegisterCarService(_repository, _unitOfWork);
    }

    [Fact]
    public void RegisterCar_WithZeroCarPrice_ThrowsCoreBusinessException()
    {
        try
        {
            CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetPrice(0)
                            .Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
            Assert.Fail("Should not get here");
        }
        catch (CoreBusinessException ex)
        {
            Assert.True(ex.Message.Equals("The price must be greater than 0"));
        }
    }

    [Fact]
    public async void RegisterCar_RegisteredAsUnsold_ThrowsCoreBusinessException()
    {
        try
        {
            CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetIsSold(true)
                            .Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);
            _repository.SaveAsync(Arg.Any<Car>()).Returns(car);
            _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            //Act
            var result = await _service.RegisterCarAsync(car, new CancellationTokenSource().Token);
            await _repository.Received().SaveAsync(Arg.Any<Car>());
            await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>()); 
            Assert.Fail("Should not get here");
        }
        catch (CoreBusinessException ex)
        {
            Assert.True(ex.Message.Equals("The car must be registered as unsold"));
        }
    }

    [Fact]
    public async void RegisterCar_RegisteredAsUnsoldAndTokenNull_ThrowsCoreBusinessException()
    {
        try
        {
            CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetIsSold(true)
                            .Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);
            _repository.SaveAsync(Arg.Any<Car>()).Returns(car);
            _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            //Act
            var result = await _service.RegisterCarAsync(car, null);
            await _repository.Received().SaveAsync(Arg.Any<Car>());
            await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>()); 
            Assert.Fail("Should not get here");
        }
        catch (CoreBusinessException ex)
        {
            Assert.True(ex.Message.Equals("The car must be registered as unsold"));
        }
    }

    [Fact]
    public void RegisterCar_WithBrandEmpty_ThrowsCoreBusinessException()
    {
        try
        {
            CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetBrand("")
                            .Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
            Assert.Fail("Should not get here");
        }
        catch (CoreBusinessException ex)
        {
            Assert.True(ex.Message.Equals("The car brand must not be empty or null"));
        }
    }

    [Fact]
    public void RegisterCar_WithVersionEmpty_ThrowsCoreBusinessException()
    {
        try
        {
            CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetVersion("")
                            .Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
            Assert.Fail("Should not get here");
        }
        catch (CoreBusinessException ex)
        {
            Assert.True(ex.Message.Equals("The car version must not be empty or null"));
        }
    }       

    [Fact]
    public void RegisterCar_WithWrongCarModel_ThrowsMinimumModelException()
    {
        try
        {
            CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetModel(2017)
                            .Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
            Assert.Fail("Should not get here");
        }
        catch (MinimumModelException ex)
        {
            Assert.True(ex.Message.Equals("The model must be older than 2018"));
        }
    }
    [Fact]
    public void RegisterCar_With2018CarModel_ThrowsMinimumModelException()
    {
        try
        {
            CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetModel(2018)
                            .Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
            Assert.Fail("Should not get here");
        }
        catch (MinimumModelException ex)
        {
            Assert.True(ex.Message.Equals("The model must be older than 2018"));
        }
    }

    [Fact]
    public void CreateCar_ModelSportsCar2020_ShouldCreateCar()
    {
        //Arrange
        CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetVersion("MX5")
                            .SetModel(2020)
                            .SetVehicleType(Enums.VehicleType.Deportivo)
                            .Build();

        //Act                    
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);

        //Assert
        var value = car.CalculatePriceVehicleType();
        Assert.True(value == car.Price);
    }

    [Fact]
    public void CreateCar_ModelSportsCar2021_ShouldCreateCar()
    {
        //Arrange
        CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetVersion("MX5")
                            .SetModel(2021)
                            .SetVehicleType(Enums.VehicleType.Deportivo)
                            .Build();

        //Act                    
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);        

        //Assert
        var value = car.CalculatePriceVehicleType();
        Assert.True(value == car.Price);
    } 

    [Fact]
    public void CreateCar_ModelSportsCar2022_ShouldCreateCar()
    {
        //Arrange
        CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetVersion("MX5")
                            .SetModel(2022)
                            .SetVehicleType(Enums.VehicleType.Deportivo)
                            .Build();

        //Act 
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);        

        //Assert
        var value = car.CalculatePriceVehicleType();
        Assert.False(value > car.Price);
    } 

    [Fact]
    public async void RegisterCarAsync_ModelSportsCar_ShouldRegisterCar()
    {
        CarDtoTest carDtoTest = new CarDtoTest.Builder()
                            .SetVersion("MX5")
                            .SetModel(2022)
                            .SetVehicleType(Enums.VehicleType.Deportivo)
                            .Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
        _repository.SaveAsync(Arg.Any<Car>()).Returns(car);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        var result = await _service.RegisterCarAsync(car, new CancellationTokenSource().Token);
        await _repository.Received().SaveAsync(Arg.Any<Car>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.Equal(car, result);
    }

    [Fact]
    public async void RegisterCarAsync_WhenCarHasModelOlderThan2018AndCancellationTokenSource_ShouldRegisterCar()
    {
        //Arrange        
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
        _repository.SaveAsync(Arg.Any<Car>()).Returns(car);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        var tokenNew = new CancellationTokenSource().Token;

        //Act
        var result = await _service.RegisterCarAsync(car, tokenNew);
        await _repository.Received().SaveAsync(Arg.Any<Car>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.Equal(car, result);
    }

    [Fact]
    public async void RegisterCarAsync_WhenCarHasModelOlderThan2018AndCancellationTokenNull_ShouldRegisterCar()
    {
        //Arrange        
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
        _repository.SaveAsync(Arg.Any<Car>()).Returns(car);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        var result = await _service.RegisterCarAsync(car, null);
        await _repository.Received().SaveAsync(Arg.Any<Car>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.Equal(car, result);
    }

    [Fact]
    public async void RegisterCarAsync_WhenCarHasModelOlderThan2018_ShouldRegisterCar()
    {
        //Arrange        
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest); 
        _repository.SaveAsync(Arg.Any<Car>()).Returns(car);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        var result = await _service.RegisterCarAsync(car, new CancellationTokenSource().Token);
        await _repository.Received().SaveAsync(Arg.Any<Car>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        //Assert
        Assert.Equal(car, result);
    }
}
