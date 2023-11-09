using StoreOnline.Domain.Dto;
using MediatR;
using StoreOnline.Domain.Enums;

namespace StoreOnline.Application.Products;

public record BuyRegisterCommand(Guid ProductId, double Price, PaymentMethod PaymentMethod) : IRequest<BuyDto>;
