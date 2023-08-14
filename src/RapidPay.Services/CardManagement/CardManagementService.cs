using System;
using System.Collections.Generic;
using System.Linq;
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
                
                //Validate if card number already exists in database
                if (await _cardRepository.IsCardDuplicated(request.Number))
                {
                    _logger.LogError($"Card {request.Number} already exists in database.");
                    return null;
                }
                
                _logger.LogInformation($"Create new card with number {request.Number}");
                return await _cardRepository.CreateNewCard(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating new card with number {request.Number}. Exception: {ex}");
                return null;
            }
            
        }

        public async Task<IEnumerable<ListCardResponse>> ListCardsAsync()
        {
            try
            {
                var cards = await _cardRepository.GetAllCards();
                return cards.Select(c => new ListCardResponse { CardNumber = c.Number, Balance = c.Balance, IdentificationNumber = c.IdentificationNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting cards. Exception: {ex}");
                return null;
            }
        }

        public async Task<CardBalanceResponse> GetCardBalance(string cardNumber, string identificationNumber)
        {
            try
            {
                //Validate if card belongs to user
                if (!await _cardRepository.IsCardAssignedToUser(cardNumber, identificationNumber))
                {
                    _logger.LogError("Card is not assigned to that user");
                    return null;
                }
                
                decimal? balance = await _cardRepository.GetCardBalance(cardNumber);
                if (!balance.HasValue)
                {
                    _logger.LogError("There's no balance in the card");
                    return null;
                }

                return new CardBalanceResponse { Balance = balance.Value, CardNumber = cardNumber };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting card balance for {cardNumber}. Exception: {ex}");
                return null;
            }
        }

        public async Task<CardPaymentResponse> ProcessPayment(DoPaymentRequest request)
        {
            _logger.LogInformation(
                $"Process a card payment card with number {request.CardNumber} and amount {request.Amount}");

            var currentBalance = await GetCardBalance(request.CardNumber, request.IdentificationNumber);
            if (currentBalance == null)
            {
                return null;
            }

            var feeToPay = await _ufeService.GetPaymentFee(_paymentFeeRepository);

            var totalToBeDiscounted = request.Amount + feeToPay;
            if (currentBalance.Balance - totalToBeDiscounted < 0)
            {
                _logger.LogError("There's not enough balance to process this payment");
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

