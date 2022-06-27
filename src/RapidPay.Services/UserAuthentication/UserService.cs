using System;
using System.Threading.Tasks;
using RapidPay.Data.Repositories;
using RapidPay.Domain;

namespace RapidPay.Services.UserAuthentication
{
	public class UserService : IUserService
	{
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _repository.Authenticate(username, password);

            if (user == null) return null;

            return user;
        }
    }
}

