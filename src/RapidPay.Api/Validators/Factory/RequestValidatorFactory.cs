using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace RapidPay.Api.Validators.Factory
{
    public interface IRequestValidatorFactory
    {
        IValidator<TRequest> GetValidator<TRequest>();
    }

    public class RequestValidatorFactory : IRequestValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RequestValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator<TRequest> GetValidator<TRequest>()
        {
            return _serviceProvider.GetService<IValidator<TRequest>>();
        }
    }
}