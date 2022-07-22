using FluentValidation;
using System;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("{Id} must be informed.");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("{UserName} must be informed.");

            RuleFor(x => x.OrderTotal)
                .GreaterThan(0)
                .WithMessage("{OrderTotal} must be greater than zero.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("{FirstName} must be informed.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("{LastName} must be informed.");

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("{EmailAddress} must be informed.")
                .EmailAddress()
                .WithMessage("{EmailAddress} must be a valid one.");

            RuleFor(x => x.AddressLine)
                .NotEmpty()
                .WithMessage("{AddressLine} must be informed.")
                .MaximumLength(100)
                .WithMessage("{AddressLine} must be up to 100 characters long.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("{Country} must be informed.")
                .MaximumLength(50)
                .WithMessage("{Country} must be up to 50 characters long.");

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("{State} must be informed.")
                .MaximumLength(50)
                .WithMessage("{State} must be up to 50 characters long.");

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("{ZipCode} must be informed.")
                .MaximumLength(15)
                .WithMessage("{ZipCode} must be up to 15 characters long.");
        }
    }
}
