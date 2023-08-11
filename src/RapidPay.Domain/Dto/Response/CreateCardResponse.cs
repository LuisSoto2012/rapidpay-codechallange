using System;
namespace RapidPay.Domain.Dto.Response
{
	public class CreateCardResponse
	{
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public string IdentificationNumber { get; set; }
    }
}

