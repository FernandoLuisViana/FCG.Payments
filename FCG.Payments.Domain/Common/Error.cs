using FCG.Payments.Domain.Enums;

public record Error(
    ErrorType Type,
    string Code,
    string Message
);
