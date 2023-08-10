using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Data.Repositories;
using RapidPay.Domain;
using RapidPay.Domain.Dto;
using RapidPay.Domain.Dto.Response;

namespace RapidPay.Services.UserAuthentication
{
	public class UserService : IUserService
	{
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<AuthenticateResponse> Authenticate(string username, string password, string key, string issuer)
        {
            var user = await _repository.Authenticate(username, password);

            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user, key, issuer);

            return new AuthenticateResponse(user, token);
        }
        
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _repository.Authenticate(username, password);

            if (user == null) return null;

            return user;
        }
        
        public async Task<User> GetById(int id)
        {
            var user = await _repository.GetById(id);

            if (user == null) return null;

            return user;
        }
        
        private string GenerateJwtToken(User user, string key, string issuer)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

