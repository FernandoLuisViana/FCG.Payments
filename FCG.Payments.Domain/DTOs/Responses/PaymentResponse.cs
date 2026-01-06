using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Domain.DTOs.Responses;

public record PaymentResponse(Guid Id, Guid OrderId, decimal TotalAmount, EPaymentMethod PaymentMethod, EPaymentStatus PaymentStatus);
