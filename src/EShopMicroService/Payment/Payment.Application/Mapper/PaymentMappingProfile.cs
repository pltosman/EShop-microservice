using System;
using AutoMapper;
using Payment.Application.Commands.CreatePayment;
using Payment.Application.Responses;

namespace Payment.Application.Mapper
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Domain.Entities.Payment, PaymentCreateCommand>().ReverseMap();
            CreateMap<Domain.Entities.Payment, PaymentResponse>().ReverseMap();
        }
    }
}
