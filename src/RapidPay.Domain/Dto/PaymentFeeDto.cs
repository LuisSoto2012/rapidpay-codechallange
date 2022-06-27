using System;
namespace RapidPay.Domain.Dto
{
	public class PaymentFeeDto
	{
        public decimal CurrentFee { get; set; }
        public DateTime FeeDate { get; set; }
    }
}

