using StoreOnline.Api.Tests.Builders;
using StoreOnline.Application.Products;
using StoreOnline.Domain.Entities;

namespace StoreOnline.Api.Tests.Anticorruption;
public class CarTestFactory
{
    protected CarTestFactory(){}
    public static Car FromCarDtoTestToModel(CarDtoTest carDto)
    {        
        return Car.CreateCar(carDto.Price, carDto.IsSold, carDto.Brand!, carDto.Version!, carDto.Model, carDto.VehicleType);
    }

    public static CarRegisterCommand FromCarDtoTestToCommand(CarDtoTest carDtoTest)
    {
        return new(carDtoTest.Price, carDtoTest.IsSold, carDtoTest.Brand!, carDtoTest.Version!, carDtoTest.Model, carDtoTest.VehicleType);
    }

    public static List<Car> Cars()
    {
        List<Car> cars = new();
        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = FromCarDtoTestToModel(carDtoTest);
        cars.Add(car);
        CarDtoTest carDtoTest1 = new CarDtoTest.Builder()
                        .SetVersion("CX50")
                        .Build();
        Car car1 = FromCarDtoTestToModel(carDtoTest1);
        cars.Add(car1);
        return cars;
    }
}