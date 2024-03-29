﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapidPay.Api.Validators;
using RapidPay.Api.Validators.Factory;
using RapidPay.Domain.Dto.Request;
using RapidPay.Domain.Dto.Response;
using RapidPay.Services.CardManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RapidPay.Api.Controllers
{
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class CardManagementController : ControllerBase
    {
        private readonly ICardManagementService _cardManagementService;
        private readonly ILogger<CardManagementController> _logger;
        private readonly IRequestValidatorFactory _validatorFactory;

        public CardManagementController(
            ICardManagementService cardManagementService,
            ILogger<CardManagementController> logger,
            IRequestValidatorFactory validatorFactory)
        {
            _cardManagementService = cardManagementService;
            _logger = logger;
            _validatorFactory = validatorFactory;
        }

        /// <summary>
        /// POST: Created new Card
        /// </summary>
        /// <param name="request">CreateCardRequest</param>
        /// <returns>Created Card database Id and Number</returns>
        [HttpPost("new-card")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateCardResponse>> CreatesCardAsync(CreateCardRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var validator = _validatorFactory.GetValidator<CreateCardRequest>();
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                
                var response = await _cardManagementService.CreateNewCard(request);

                if (response != null)
                    return CreatedAtAction("GetsCardBalance",
                        new {cardNumber = response.CardNumber, identificationNumber = response.IdentificationNumber},
                        response);
                var logError = $"Error when creating new card with number: {request.Number}";
                _logger.LogError(logError);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);

            }
            catch (System.Exception ex)
            {
                var logError = $"Error when creating new card with number: {request.Number}. Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }

        }
        
        /// <summary>
        /// GET: List all cards
        /// </summary>
        /// <returns>Created Cards in database with Number and Balance</returns>
        [HttpPost("list-cards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ListCardResponse>>> ListCardsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var cards = await _cardManagementService.ListCardsAsync();
                if (cards == null)
                {
                    return NotFound(new { message = "Created cards do not exist" });
                }

                return Ok(cards);
            }
            catch (System.Exception ex)
            {
                var logError = $"Error when list all cards in database. Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }

        }

        /// <summary>
        /// PUT: Execute a payment using a given card number and the payment
        /// </summary>
        /// <param name="paymentRequest">Card Number and Amount to be paid</param>
        /// <returns></returns>
        [HttpPut("card/payment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CardPaymentResponse>> PaymentAsync(
            [FromBody] DoPaymentRequest paymentRequest
        )
        {
            var validator = _validatorFactory.GetValidator<DoPaymentRequest>();
            var validationResult = await validator.ValidateAsync(paymentRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            CardPaymentResponse response;
            try
            {
                response = await _cardManagementService.ProcessPayment(paymentRequest);
            }
            catch (System.Exception ex)
            {
                var logError =
                    $"Error doing a card payment with number: {paymentRequest.CardNumber} " +
                    $"and amount: {paymentRequest.Amount}. " +
                    $"Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }

            return Accepted(response);
        }

        /// <summary>
        /// GET: Gets Card Balance
        /// </summary>
        /// <param name="cardNumber">15 digits card number</param>
        /// <param name="identificationNumber">person's identification number</param>
        /// <returns>CardBalanceResponse</returns>
        [HttpGet("card/{cardNumber}/{identificationNumber}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CardBalanceResponse>> GetsCardBalance([FromRoute] string cardNumber, [FromRoute] string identificationNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 15 || string.IsNullOrEmpty(identificationNumber))
            {
                return BadRequest();
            }
            try
            {
                var card = await _cardManagementService.GetCardBalance(cardNumber, identificationNumber);
                if (card == null)
                {
                    return NotFound(new { message = "Card with provided does not exist" });
                }

                return Ok(card);
            }
            catch (System.Exception ex)
            {
                var logError = $"Error retrieving card with number: {cardNumber}. Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }

        }
    }
}

