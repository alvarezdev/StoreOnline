using StoreOnline.Domain.Enums;
using StoreOnline.Domain.Exceptions;

namespace StoreOnline.Domain.Entities;
public class Buy : DomainEntity
{
    const double TEN_PERCENT = 0.1;
    const int TEN = 10;
    const int FIFTEEN = 15;
    const double FIVE_PERCENT = 0.05;
    const string ID_EMPTY_EXCEPTION_MESSAGE = "The car id must not be empty";
    const string PAYMENT_METHOD_EXCEPTION_MESSAGE = "The payment method is not correct";
    const string PRICE_EXCEPTION_MESSAGE = "The price must be greater than 0";

    private Buy(Guid productId, double price, PaymentMethod paymentMethod)
    {
        var guidEmpty = Guid.Empty;
        ProductId = productId != guidEmpty ? productId : throw new CoreBusinessException(ID_EMPTY_EXCEPTION_MESSAGE);
        PaymentMethod = ValidatePaymentMethod(paymentMethod);
        Price = price > 0 ? price : throw new CoreBusinessException(PRICE_EXCEPTION_MESSAGE);        
    }

    public static Buy CreateBuy(Guid productId, double price, PaymentMethod paymentMethod)
    {
        return new Buy(productId, price, paymentMethod);
    }

    private static PaymentMethod ValidatePaymentMethod(PaymentMethod paymentMethod)
    {
        PaymentMethod[] valores = (PaymentMethod[])Enum.GetValues(typeof(PaymentMethod));
        return valores.Any(x => x.Equals(paymentMethod)) ? paymentMethod : throw new CoreBusinessException(PAYMENT_METHOD_EXCEPTION_MESSAGE);
    }

    public void CalculatePrice(double price, int day)
    {
        Price = Price + CalculatePriceByPaymentMethod(price) - CalculatePriceByBuyDate(price, day);
    }

    private double CalculatePriceByPaymentMethod(double price)
    {
        return PaymentMethod.Equals(PaymentMethod.CreditCar) ? price * FIVE_PERCENT : 0;
    }

    private double CalculatePriceByBuyDate(double price, int day)
    {
        bool validateDay = day == TEN || day == FIFTEEN;
        return validateDay ? price * TEN_PERCENT : 0;
    } 

    public Guid ProductId { get; init; }
    public double Price { get; set; }
    public PaymentMethod PaymentMethod { get; init; }
}
