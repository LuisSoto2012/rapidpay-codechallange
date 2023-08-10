using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RapidPay.Domain
{
    /// <summary>
    /// Card
    /// </summary>
	public class Card
	{
        public int Id { get; set; }
        /// <summary>
        /// Card Number
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Card Balance
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// Card Payment Histories
        /// </summary>
        public ICollection<PaymentHistory> PaymentHistories { get; set; }

        public Card(string number, decimal balance)
        {
            ValidateFields(number, balance);
            Number = number;
            Balance = balance;
        }

        private void ValidateFields(string number, decimal balance)
        {
            if (number.Length != 15)
                throw new ArgumentException("Credit card number must have 15 digits");

            var isMatch = Regex.IsMatch(number, @"^\d+$");
            if (!isMatch)
                throw new ArgumentException("Credit card number must have only digits");
            
            if (balance <= 0)
                throw new ArgumentException("Credit card balance must be greater than zero");
        }
    }
}

