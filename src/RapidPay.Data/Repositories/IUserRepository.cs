using System;
using System.Threading.Tasks;
using RapidPay.Domain;

namespace RapidPay.Data.Repositories
{
	public interface IUserRepository
	{
        public Task<User> Authenticate(string username, string password);
    }
}

