using System;
namespace RapidPay.Domain
{
    /// <summary>
    /// User
    /// </summary>
	public class User
	{
        public int Id { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}

