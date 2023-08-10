using System;
using Bogus;
using FluentAssertions;
using RapidPay.Domain;
using Xunit;

namespace RapidPay.Test
{
    public class CreditCardTests
    {
        private readonly Faker _faker;

        public CreditCardTests()
        {
            _faker = new Faker();
        }
        
        [Fact]
        public void NewCard_InvalidNumber_8digits_ThrowsArgumentException()
        {
            // Arrange
            const string invalidNumber = "12345678";
            var balance = _faker.Finance.Amount();

            // Act
            Action act = () => new Card(invalidNumber, balance);
        
            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Credit card number must have 15 digits");
        }
        
        [Fact]
        public void NewCard_InvalidNumber_AlphanumericValue_ThrowsArgumentException()
        {
            // Arrange
            const string invalidNumber = "abcd1234defg123";
            var balance = _faker.Finance.Amount();

            // Act
            Action act = () => new Card(invalidNumber, balance);
        
            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Credit card number must have only digits");
        }
        
        [Fact]
        public void NewCard_InvalidBalance_NegativeValue_ThrowsArgumentException()
        {
            // Arrange
            const string number = "123456789123456";
            const decimal invalidBalance = -200;

            // Act
            Action act = () => new Card(number, invalidBalance);
        
            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Credit card balance must be greater than zero");
        }
    }
}