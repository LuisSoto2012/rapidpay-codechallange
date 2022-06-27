using System;
namespace RapidPay.Data.Extensions
{
	public static class DecimalExtensions
	{
        public static decimal RandomNumberBetween(this Random rnd, double minValue, double maxValue)
        {
            return (decimal)(rnd.NextDouble() * (maxValue - minValue));
        }
    }
}

