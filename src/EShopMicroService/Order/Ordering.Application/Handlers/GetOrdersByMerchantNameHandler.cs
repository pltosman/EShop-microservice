using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    public class GetOrdersByMerchantNameHandler : IRequestHandler<GetOrdersByMerchantNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersByMerchantNameHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByMerchantNameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByMerchantName(request.MerchantName);

            var response = _mapper.Map<IEnumerable<OrderResponse>>(orderList);

            return response;
        }
    }
}