using FCG.Payments.Domain.Common;
using FCG.Payments.Domain.DTOs.Requests;
using FCG.Payments.Domain.DTOs.Responses;

namespace FCG.Payments.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task<Result<PaymentResponse>> ProcessPaymentAsync(ProcessPaymentRequest request);
    Task<Result<List<PaymentResponse>>> ListAsync();
}
