using StoreOnline.Domain.Enums;

namespace StoreOnline.Domain.Dto;

public record CarDto(Guid Id, double Price, bool IsSold, string Brand, string Version, int Model, VehicleType VehicleType);
