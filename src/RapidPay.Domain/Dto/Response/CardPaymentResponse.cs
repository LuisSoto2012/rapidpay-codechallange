using System;
namespace RapidPay.Domain.Dto.Response
{
	public class CardPaymentResponse
	{
        public CardPaymentResponse(string cardNumber)
        {
            CardNumber = cardNumber;
        }
        public string CardNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal FeePaid { get; set; }
    }
}

