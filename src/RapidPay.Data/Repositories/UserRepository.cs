using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain;

namespace RapidPay.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
        private readonly RapidPayContext _context;

		public UserRepository(RapidPayContext context)
		{
            _context = context ?? throw new ArgumentNullException(nameof(context));
		}

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            if (user == null)
            {
                return null;
            }

            user.Password = null;
            return user;
        }
    }
}

