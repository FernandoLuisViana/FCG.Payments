using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Domain.DTOs.Requests;

public record ProcessPaymentRequest(Guid OrderId, decimal TotalAmount, EPaymentMethod PaymentMethod);
