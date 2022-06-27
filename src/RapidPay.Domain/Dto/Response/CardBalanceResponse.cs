using System;
namespace RapidPay.Domain.Dto.Response
{
	public class CardBalanceResponse
	{
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
    }
}

