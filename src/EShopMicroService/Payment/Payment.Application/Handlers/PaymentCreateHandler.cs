using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EShop.EventBus.Abstractions;
using MediatR;
using P_IntegrationEvents.Events;
using Payment.Application.Commands.CreatePayment;
using Payment.Application.Responses;
using Payment.Domain.Repositories;

namespace Payment.Application.Handlers
{
    public class PaymentCreateHandler : IRequestHandler<PaymentCreateCommand, PaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private IMapper _mapper;
        private readonly IEventBus _eventBus;
        public PaymentCreateHandler(IPaymentRepository paymentRepository, IMapper mapper, IEventBus eventBus)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task<PaymentResponse> Handle(PaymentCreateCommand request, CancellationToken cancellationToken)
        {
           
            if (request.OrderId == Guid.Empty)
                throw new ApplicationException("Order Id could not be Empty.");

            var paymentEntity = _mapper.Map<Domain.Entities.Payment>(request);

            if (paymentEntity == null)
                throw new ApplicationException("Entity could not be mapped");

            var payment = await _paymentRepository.AddAsync(paymentEntity);

            var paymentResponse = _mapper.Map<PaymentResponse>(payment);


            _eventBus.Publish(new OrderStatusEvent(request.OrderId, Ordering.Domain.Enums.OrderStatus.OrderConfirmed));


            return paymentResponse;
        }
    }
}
