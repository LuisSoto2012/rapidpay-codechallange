using System;
using System.Threading.Tasks;
using RapidPay.Domain.Dto.Request;
using RapidPay.Domain.Dto.Response;

namespace RapidPay.Services.CardManagement
{
	public interface ICardManagementService
	{
        public Task<CreateCardResponse> CreateNewCard(CreateCardRequest request);
        Task<CardPaymentResponse> ProcessPayment(DoPaymentRequest request);
        public Task<CardBalanceResponse> GetCardBalance(string cardNumber);
    }
}

