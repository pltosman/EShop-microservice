using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Commands.OrderStatus;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    public class OrderStatusHandler : IRequestHandler<OrderStatusCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;
        public OrderStatusHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(OrderStatusCommand request, CancellationToken cancellationToken)
        {

            if (request.OrderId == null)
                throw new ApplicationException("OrderID could not be null");

            var orderEntity = await _orderRepository.GetByIdAsync(request.OrderId);

            if (orderEntity == null) throw new ApplicationException("Order could not find");

            orderEntity.OrderStatus = request.OrderStatus;

            await _orderRepository.UpdateAsync(orderEntity);

            var orderResponse = _mapper.Map<OrderResponse>(orderEntity);

            return orderResponse;
        }

    }
}