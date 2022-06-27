using System;
using System.Collections.Generic;

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
    }
}

