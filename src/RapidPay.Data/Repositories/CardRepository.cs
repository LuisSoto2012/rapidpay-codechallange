using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain;
using RapidPay.Domain.Dto.Request;
using RapidPay.Domain.Dto.Response;

namespace RapidPay.Data.Repositories
{
	public class CardRepository : ICardRepository
	{
        private readonly RapidPayContext _context;
        private readonly IMapper _mapper;

		public CardRepository(RapidPayContext context, IMapper mapper)
		{
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

        public async Task<CreateCardResponse> CreateNewCard(CreateCardRequest request)
        {
            try
            {
                var newCard = _mapper.Map<Card>(request);
                _context.Add(newCard);
                await _context.SaveChangesAsync();
                return new CreateCardResponse() { CardId = newCard.Id, CardNumber = newCard.Number };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<IEnumerable<Card>> GetAllCards()
        {
            try
            {
                var cards = await _context.Cards.ToListAsync();
                if (!cards.Any())
                    return null;
                return cards;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<decimal?> GetCardBalance(string cardNumber)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Number == cardNumber);
            if (card == null)
            {
                return null;
            }
            return card.Balance;
        }

        public async Task<bool> UpdateBalance(string cardNumber, decimal amount)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Number == cardNumber);
            if (card == null)
            {
                return false;
            }
            try
            {
                card.Balance -= amount;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
            return true;
        }

        public async Task<bool> SaveCardPaymentTransaction(string cardNumber, decimal payment, decimal fee)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Number == cardNumber);
            if (card == null)
            {
                return false;
            }
            try
            {
                var paymentTransaction = new PaymentHistory
                {
                    Payment = payment,
                    Fee = fee,
                    PaymentDate = DateTime.Now,
                    CardId = card.Id,
                };
                _context.Add(paymentTransaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }     
            return true;
        }   
    }
}

