using System;
namespace RapidPay.Domain
{
    /// <summary>
    /// Payment Fee
    /// </summary>
	public class PaymentFee
	{
        public int Id { get; set; }
        /// <summary>
        /// Fee Amount
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Fee Register Date
        /// </summary>
        public DateTime FeeDate { get; set; }
    }
}

