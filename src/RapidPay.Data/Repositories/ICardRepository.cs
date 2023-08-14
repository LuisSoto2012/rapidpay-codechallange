using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RapidPay.Domain;
using RapidPay.Domain.Dto.Request;
using RapidPay.Domain.Dto.Response;

namespace RapidPay.Data.Repositories
{
	public interface ICardRepository
	{
        Task<CreateCardResponse> CreateNewCard(CreateCardRequest request);
        Task<IEnumerable<Card>> GetAllCards();
        Task<decimal?> GetCardBalance(string cardNumber);
        Task<bool> UpdateBalance(string cardNumber, decimal amount);
        Task<bool> SaveCardPaymentTransaction(string cardNumber, decimal payment, decimal fee);
        Task<bool> IsCardAssignedToUser(string cardNumber, string identificationNumber);
        Task<bool> IsCardDuplicated(string cardNumber);
	}
}

