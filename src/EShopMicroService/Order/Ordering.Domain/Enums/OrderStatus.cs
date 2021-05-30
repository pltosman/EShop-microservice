namespace Ordering.Domain.Enums
{
    public enum OrderStatus
    {
        OrderCancelled,
        WaitingForPayment,
        OrderPaymentSuccess,
        OrderConfirmed,
        OrderShipped,
        OrderDelivered,
        OrderReturned,
        OrderProblem
    }
}
