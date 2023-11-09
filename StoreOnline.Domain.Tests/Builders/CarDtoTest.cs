using StoreOnline.Domain.Enums;

namespace StoreOnline.Domain.Tests.Builders;

public class CarDtoTest
{
    public double Price = 100000000;
    public bool IsSold = false;
    public string? Brand = "Mazda";
    public string? Version = "CX30";
    public int Model = 2022;
    public VehicleType VehicleType = VehicleType.Suv;

    public class Builder
    {
        private CarDtoTest car;

        public Builder()
        {
            car = new CarDtoTest();
        }

        public Builder SetPrice(double price)
        {
            car.Price = price;
            return this;
        }

        public Builder SetIsSold(bool isSold)
        {
            car.IsSold = isSold;
            return this;
        }

        public Builder SetBrand(string brand)
        {
            car.Brand = brand;
            return this;
        }

        public Builder SetVersion(string version)
        {
            car.Version = version;
            return this;
        }

        public Builder SetModel(int model)
        {
            car.Model = model;
            return this;
        }

        public Builder SetVehicleType(VehicleType vehicleType)
        {
            car.VehicleType = vehicleType;
            return this;
        }

        public CarDtoTest Build()
        {
            return car;
        }
    } 

}