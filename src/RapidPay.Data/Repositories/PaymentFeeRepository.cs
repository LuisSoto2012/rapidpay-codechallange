using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain;
using RapidPay.Domain.Dto;

namespace RapidPay.Data.Repositories
{
	public class PaymentFeeRepository : IPaymentFeeRepository
	{
        private readonly RapidPayContext _context;
        private readonly IMapper _mapper;

		public PaymentFeeRepository(RapidPayContext context, IMapper mapper)
		{
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

        public async Task<bool> CreateNewPaymentFee(decimal fee)
        {
            try
            {
                var paymentFee = new PaymentFee
                {
                    Fee = fee,
                    FeeDate = DateTime.Now
                };
                _context.Add(paymentFee);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public async Task<PaymentFeeDto> GetLastPaymentFee()
        {
            //Get Last Payment Fee
            var paymentFee = await _context.PaymentFees
                                        .OrderBy(x => x.FeeDate)
                                        .Take(1)
                                        .SingleOrDefaultAsync();
            if (paymentFee == null)
            {
                return default;
            }
            return _mapper.Map<PaymentFeeDto>(paymentFee);
        }
    }
}

