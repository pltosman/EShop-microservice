using System;
namespace Payment.Domain
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
