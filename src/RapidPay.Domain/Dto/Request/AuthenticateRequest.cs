using System;
namespace RapidPay.Domain.Dto.Request
{
	public class AuthenticateRequest
	{
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

