using System;
using AutoMapper;
using RapidPay.Domain;
using RapidPay.Domain.Dto;
using RapidPay.Domain.Dto.Request;

namespace RapidPay.Data.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			//Card
			CreateMap<CreateCardRequest, Card>();

			//PaymentFee
			CreateMap<PaymentFee, PaymentFeeDto>()
				.ForMember(r => r.CurrentFee, x => x.MapFrom(p => p.Fee));
        }
	}
}

