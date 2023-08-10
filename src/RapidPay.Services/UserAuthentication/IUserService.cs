using System;
using System.Threading.Tasks;
using RapidPay.Domain;
using RapidPay.Domain.Dto.Response;

namespace RapidPay.Services.UserAuthentication
{
	public interface IUserService
	{
        Task<AuthenticateResponse> Authenticate(string username, string password, string key, string issuer);
        Task<User> Authenticate(string username, string password);
        Task<User> GetById(int id);
	}
}

