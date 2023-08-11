using System;
namespace RapidPay.Domain.Dto.Request
{
	public class DoPaymentRequest
	{
        public string CardNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public decimal Amount { get; set; }
    }
}

