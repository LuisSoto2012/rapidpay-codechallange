using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RapidPay.Domain.Dto.Request;
using RapidPay.Domain.Dto.Response;

namespace RapidPay.Services.CardManagement
{
	public interface ICardManagementService
	{
        public Task<CreateCardResponse> CreateNewCard(CreateCardRequest request);
        Task<IEnumerable<ListCardResponse>> ListCardsAsync();
        Task<CardPaymentResponse> ProcessPayment(DoPaymentRequest request);
        public Task<CardBalanceResponse> GetCardBalance(string cardNumber);
    }
}

