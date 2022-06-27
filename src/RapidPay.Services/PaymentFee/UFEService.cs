using System;
using System.Threading.Tasks;
using RapidPay.Data.Extensions;
using RapidPay.Data.Repositories;

namespace RapidPay.Services.PaymentFee
{
	public class UFEService : IUFEService
	{
        //private readonly IPaymentFeeRepository _paymentFeeRepository;

		public UFEService()
		{
            //_paymentFeeRepository = paymentFeeRepository ?? throw new ArgumentNullException(nameof(paymentFeeRepository));
		}

        public async Task<decimal> GetPaymentFee(IPaymentFeeRepository paymentFeeRepository)
        {
            var lastFee = await paymentFeeRepository.GetLastPaymentFee();
            decimal paymentFee = lastFee.CurrentFee;
            if ((lastFee.FeeDate - DateTime.UtcNow).TotalHours > 1)
            {
                var newFee = new Random().RandomNumberBetween(0.0, 2.0);
                if (await paymentFeeRepository.CreateNewPaymentFee(newFee))
                {
                    paymentFee = newFee * (newFee == default ? 1 : newFee);
                }
            }

            return paymentFee;
        }
    }
}

