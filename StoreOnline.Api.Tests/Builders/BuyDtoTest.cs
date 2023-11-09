using StoreOnline.Domain.Enums;

namespace StoreOnline.Api.Tests.Builders;
public class BuyDtoTest
{
    public Guid ProductId = Guid.NewGuid();
    public double Price = 0;
    public PaymentMethod PaymentMethod = PaymentMethod.DebitCar;

    public class Builder
    {
        private BuyDtoTest buy;

        public Builder()
        {
            buy = new BuyDtoTest();
        }

        public Builder SetProductId(Guid productId)
        {
            buy.ProductId = productId;
            return this;
        }

        public Builder SetPrice(double price)
        {
            buy.Price = price;
            return this;
        }

        public Builder SetPaymentMethod(PaymentMethod paymentMethod)
        {
            buy.PaymentMethod = paymentMethod;
            return this;
        }

        public BuyDtoTest Build()
        {
            return buy;
        }
    }
}