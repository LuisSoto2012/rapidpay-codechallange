using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidPay.Data.Extensions;
using RapidPay.Domain;

namespace RapidPay.Data
{
	public class RapidPayContextSeed
	{
        public static async Task SeedAsync(RapidPayContext rapidPayContext, ILogger<RapidPayContextSeed> logger)
        {
            //Seed User
            if (!rapidPayContext.Users.Any())
            {
                rapidPayContext.Users.AddRange(GetPreconfiguredUsers());
                await rapidPayContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(RapidPayContext).Name);
            }

            //Seed Fee
            if (!rapidPayContext.PaymentFees.Any())
            {
                rapidPayContext.PaymentFees.Add(GetPreconfiguredFee());
                await rapidPayContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(RapidPayContext).Name);
            }
        }

        private static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>
            {
                new User()
                {
                    Username = "dummy", Password = "dummy"
                }
            };
        }

        private static PaymentFee GetPreconfiguredFee()
        {
            var rand = new Random();
            return new PaymentFee { Fee = rand.RandomNumberBetween(0.0, 2.0), FeeDate = DateTime.UtcNow };
        }
    }
}

