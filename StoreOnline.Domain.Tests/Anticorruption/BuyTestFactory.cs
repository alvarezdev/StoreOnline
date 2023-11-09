using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Tests.Builders;

public class BuyTestFactory
{
    protected BuyTestFactory(){}
    public static Buy FromBuyDtoTestToModel(BuyDtoTest buyDto)
    {        
        return Buy.CreateBuy(buyDto.ProductId, buyDto.Price, buyDto.PaymentMethod);
    }
}