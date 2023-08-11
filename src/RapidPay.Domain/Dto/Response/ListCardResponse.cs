namespace RapidPay.Domain.Dto.Response
{
    public class ListCardResponse
    {
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public string IdentificationNumber { get; set; }
    }
}