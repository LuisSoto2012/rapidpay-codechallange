using System;
namespace RapidPay.Domain
{
    /// <summary>
    /// PaymentHistory
    /// </summary>
	public class PaymentHistory
	{
        public int Id { get; set; }
        /// <summary>
        /// Payment Amount
        /// </summary>
        public decimal Payment { get; set; }
        /// <summary>
        /// Applied Fee Amount
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Payment Date
        /// </summary>
        public DateTime PaymentDate { get; set; }
        /// <summary>
        /// Card Id Foreign Key
        /// </summary>
        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}

