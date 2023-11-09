using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Tests.Builders;

public class CarTestFactory
{
    protected CarTestFactory(){}
    public static Car FromCarDtoTestToModel(CarDtoTest carDto)
    {        
        return Car.CreateCar(carDto!.Price, carDto!.IsSold, carDto!.Brand!, carDto!.Version!, carDto!.Model, carDto!.VehicleType);
    }
}