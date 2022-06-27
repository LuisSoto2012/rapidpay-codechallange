using System;
namespace RapidPay.Domain.Dto.Request
{
	public class CreateCardRequest
	{
        public string Number { get; set; }
        public decimal Balance { get; set; }
    }
}

