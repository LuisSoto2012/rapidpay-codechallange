using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidPay.Data.Repositories;
using RapidPay.Domain.Dto.Request;
using RapidPay.Domain.Dto.Response;
using RapidPay.Services.PaymentFee;

namespace RapidPay.Services.CardManagement
{
	public class CardManagementService : ICardManagementService
	{
        private readonly ICardRepository _cardRepository;
        private readonly IPaymentFeeRepository _paymentFeeRepository;
        private readonly IUFEService _ufeService;
        private readonly ILogger<CardManagementService> _logger;

        public CardManagementService(ICardRepository cardRepository, IPaymentFeeRepository paymentFeeRepository,IUFEService ufeService, ILogger<CardManagementService> logger)
		{
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _paymentFeeRepository = paymentFeeRepository ?? throw new ArgumentNullException(nameof(paymentFeeRepository));
            _ufeService = ufeService ?? throw new ArgumentNullException(nameof(ufeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateCardResponse> CreateNewCard(CreateCardRequest request)
        {
            try
            {
                _logger.LogInformation($"Create new card with number {request.Number}");
                return await _cardRepository.CreateNewCard(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating new card with numer {request.Number}. Exception: {ex}");
                return null;
            }
            
        }

        public async Task<CardBalanceResponse> GetCardBalance(string cardNumber)
        {
            decimal? balance = await _cardRepository.GetCardBalance(cardNumber);
            if (!balance.HasValue)
            {
                return null;
            }

            return new CardBalanceResponse { Balance = balance.Value, CardNumber = cardNumber };
        }

        public async Task<CardPaymentResponse> ProcessPayment(DoPaymentRequest request)
        {
            _logger.LogInformation(
                $"Process a card payment card with number {request.CardNumber} and amount {request.Amount}");
            var currentBalance = await GetCardBalance(request.CardNumber);
            if (currentBalance == null)
            {
                _logger.LogError("There's no balance in the card");
                return null;
            }

            var feeToPay = await _ufeService.GetPaymentFee(_paymentFeeRepository);

            var totalToBeDiscounted = request.Amount + feeToPay;
            if (currentBalance.Balance - totalToBeDiscounted < 0)
            {
                _logger.LogError("There's not enough balance  to process this payment");
                return null;
            }

            var response = new CardPaymentResponse(request.CardNumber);

            try
            {
                if (await _cardRepository.SaveCardPaymentTransaction(request.CardNumber, request.Amount, feeToPay))
                {
                    await _cardRepository.UpdateBalance(request.CardNumber, request.Amount + feeToPay);
                    response.AmountPaid = request.Amount;
                    response.FeePaid = feeToPay;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing new payment for Card {request.CardNumber}. Exception: {ex}");
                return null;
            }  

            return response;
        }
    }
}

