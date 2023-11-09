using StoreOnline.Application.Products;
using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Entities;

namespace StoreOnline.Application.Anticorruption;

public class CarFactory
{
    protected CarFactory(){}
    public static Car FromCarCommandToModel(CarRegisterCommand carDto)
    {       
        return Car.CreateCar(carDto!.Price, carDto!.IsSold, carDto!.Brand, carDto!.Version, carDto!.Model, carDto!.VehicleType);
    }

    public static CarDto FromCarModelToDto(Car car)
    {
        return new CarDto(car!.Id, car!.Price, car!.IsSold, car!.Brand, car!.Version, car!.Model, car!.VehicleType);
    }

    public static IEnumerable<CarDto> FromCarListModelToDto(IEnumerable<Car> list)
    {
        List<CarDto> listReturn = new();
        foreach (var item in list)
        {
            var product = FromCarModelToDto(item);
            listReturn.Add(product);
        }
        return listReturn;
    }

}
