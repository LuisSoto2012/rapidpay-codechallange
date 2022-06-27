using System;
using System.Threading.Tasks;
using RapidPay.Domain;

namespace RapidPay.Services.UserAuthentication
{
	public interface IUserService
	{
        Task<User> Authenticate(string username, string password);
    }
}

