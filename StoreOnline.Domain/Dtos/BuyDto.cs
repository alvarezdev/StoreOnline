using StoreOnline.Domain.Enums;

namespace StoreOnline.Domain.Dto;

public record BuyDto(Guid ProductId, double Price, PaymentMethod PaymentMethod);
