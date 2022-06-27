using System;
using System.Threading.Tasks;
using RapidPay.Data.Repositories;

namespace RapidPay.Services.PaymentFee
{
	public interface IUFEService
	{
        Task<decimal> GetPaymentFee(IPaymentFeeRepository paymentFeeRepository);
    }
}

