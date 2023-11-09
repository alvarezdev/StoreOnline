using StoreOnline.Domain.Enums;
using StoreOnline.Domain.Exceptions;

namespace StoreOnline.Domain.Entities;
public class Car : DomainEntity
{
    const string PRICE_EXCEPTION_MESSAGE = "The price must be greater than 0";
    const string BRAND_EXCEPTION_MESSAGE = "The car brand must not be empty or null";
    const string VERSION_EXCEPTION_MESSAGE = "The car version must not be empty or null";
    const string MINIMUM_MODEL_EXCEPTION_MESSAGE = "The model must be older than 2018";
    const string VEHICLE_TYPE_EXCEPTION_MESSAGE = "The vehicle type is not correct";
    const int MINIMUM_MODEL = 2018;
    const double FIFTEEN_PERCENT = 0.15;
    const int MINIMUM_MODEL_SPORTS_CAR = 2021;
    private Car (double price, bool isSold, string brand, string version, int model, VehicleType vehicleType)
    {
        Price = price > 0 ? price : throw new CoreBusinessException(PRICE_EXCEPTION_MESSAGE);
        IsSold = isSold;
        Brand = !string.IsNullOrEmpty(brand) ? brand : throw new CoreBusinessException(BRAND_EXCEPTION_MESSAGE); 
        Version = !string.IsNullOrEmpty(version) ? version : throw new CoreBusinessException(VERSION_EXCEPTION_MESSAGE);
        Model = model <= MINIMUM_MODEL ? throw new MinimumModelException(MINIMUM_MODEL_EXCEPTION_MESSAGE) : model;
        VehicleType = ValidateVehicleType(vehicleType);
    }

    public static Car CreateCar(double price, bool isSold, string brand, string version, int model, VehicleType vehicleType)
    {
        return new(price, isSold, brand, version, model,vehicleType);
    }

    private static VehicleType ValidateVehicleType(VehicleType vehicleType)
    {
        VehicleType[] valores = (VehicleType[])Enum.GetValues(typeof(VehicleType));        
        return valores.Any(x => x.Equals(vehicleType)) ? vehicleType : throw new CoreBusinessException(VEHICLE_TYPE_EXCEPTION_MESSAGE);
    }

    public double CalculatePriceVehicleType()
    {   
        double value = Price;
        switch (VehicleType)
        {
            case VehicleType.Deportivo:
                if (Model > MINIMUM_MODEL_SPORTS_CAR)
                {
                    value = Price * FIFTEEN_PERCENT;
                }
                break;
            default:
                value = Price;
                break;    
        }  
        return value;
    }

    public double Price {get; set;}
    public bool IsSold {get; set;}
    public string Brand {get; init;}
    public string Version {get; init;}
    public int Model {get; init;}
    public VehicleType VehicleType {get; init;}
}
