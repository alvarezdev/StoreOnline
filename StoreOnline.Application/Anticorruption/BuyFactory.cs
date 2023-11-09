using StoreOnline.Application.Products;
using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Entities;

namespace StoreOnline.Application.Anticorruption;
public class BuyFactory
{
    protected BuyFactory(){}

    public static Buy FromBuyCommandToModel(BuyRegisterCommand buyRegisterCommand)
    {
        var buy = Buy.CreateBuy(buyRegisterCommand!.ProductId, buyRegisterCommand!.Price, buyRegisterCommand!.PaymentMethod);
        buy!.CalculatePrice(buy!.Price, DateTime.Today.Day - 1);       
        return buy;
    }

    public static BuyDto FromBuyModelToDto(Buy buy)
    {
        return new (buy!.ProductId, buy!.Price, buy!.PaymentMethod);
    }
}
