using System;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Domain.Dto.Request
{
	public class AuthenticateRequest
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
    }
}

