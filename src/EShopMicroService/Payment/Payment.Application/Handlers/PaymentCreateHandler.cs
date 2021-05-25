using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Payment.Application.Commands.CreatePayment;
using Payment.Application.Responses;
using Payment.Domain.Repositories;

namespace Payment.Application.Handlers
{
    public class PaymentCreateHandler : IRequestHandler<PaymentCreateCommand, PaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private IMapper _mapper;
        public PaymentCreateHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<PaymentResponse> Handle(PaymentCreateCommand request, CancellationToken cancellationToken)
        {
            var paymentEntity = _mapper.Map<Domain.Entities.Payment>(request);

            if (paymentEntity == null)
                throw new ApplicationException("Entity could not be mapped");

            var payment = await _paymentRepository.AddAsync(paymentEntity);

            var paymentResponse = _mapper.Map<PaymentResponse>(payment);

            return paymentResponse;
        }
    }
}
