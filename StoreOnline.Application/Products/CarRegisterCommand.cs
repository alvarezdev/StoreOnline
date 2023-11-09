using StoreOnline.Domain.Dto;
using MediatR;
using StoreOnline.Domain.Enums;

namespace StoreOnline.Application.Products;

public record CarRegisterCommand(double Price, bool IsSold, string Brand, string Version, int Model, VehicleType VehicleType) : IRequest<CarDto>;
