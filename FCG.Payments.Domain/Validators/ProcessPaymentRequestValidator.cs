using FCG.Payments.Domain.DTOs.Requests;
using FluentValidation;

namespace FCG.Payments.Domain.Validators;

public class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
{
    public ProcessPaymentRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotNull().WithMessage("Order Id is required.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method.");
    }
}
