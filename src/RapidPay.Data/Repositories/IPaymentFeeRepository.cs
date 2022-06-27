using System;
using System.Threading.Tasks;
using RapidPay.Domain.Dto;

namespace RapidPay.Data.Repositories
{
	public interface IPaymentFeeRepository
	{
        Task<bool> CreateNewPaymentFee(decimal fee);
        Task<PaymentFeeDto> GetLastPaymentFee();
    }
}

