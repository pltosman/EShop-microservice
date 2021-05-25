using System.Collections.Generic;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries
{
    public class GetOrdersByMerchantNameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string MerchantName { get; set; }

        public GetOrdersByMerchantNameQuery(string merchantName)
        {
            MerchantName = merchantName;
        }
    }
}