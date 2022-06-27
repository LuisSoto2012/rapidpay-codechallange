using System;
using System.Threading.Tasks;
using RapidPay.Domain.Dto.Request;
using RapidPay.Domain.Dto.Response;

namespace RapidPay.Data.Repositories
{
	public interface ICardRepository
	{
        Task<CreateCardResponse> CreateNewCard(CreateCardRequest request);
        Task<decimal?> GetCardBalance(string cardNumber);
        Task<bool> UpdateBalance(string cardNumber, decimal amount);
        Task<bool> SaveCardPaymentTransaction(string cardNumber, decimal payment, decimal fee);        
    }
}

