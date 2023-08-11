using System;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Domain.Dto.Request
{
	public class CreateCardRequest
	{
		[Required]
        public string Number { get; set; }
        public decimal Balance { get; set; }
        [Required]
        public string IdentificationNumber { get; set; }
    }
}

