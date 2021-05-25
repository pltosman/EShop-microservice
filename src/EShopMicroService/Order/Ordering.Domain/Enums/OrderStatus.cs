namespace Ordering.Domain.Enums
{
   public enum OrderStatus
    {
        WaitingForPayment,
        OrderPaymentSuccess,   
        OrderConfirmed,
        OrderShipped,
        OrderDelivered
    }
}