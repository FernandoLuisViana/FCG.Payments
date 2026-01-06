using FCG.Payments.Domain.Common;
using FCG.Payments.Domain.DTOs.Requests;
using FCG.Payments.Domain.DTOs.Responses;
using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Enums;
using FCG.Payments.Domain.Errors;
using FCG.Payments.Domain.Interfaces.Repositories;
using FCG.Payments.Domain.Interfaces.Services;
using System.Linq;

namespace FCG.Payments.Services.Payment;

public class PaymentService(IPaymentRepository repository) : IPaymentService
{
    public async Task<Result<PaymentResponse>> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        var payment = new PaymentEntity(request.OrderId, request.TotalAmount, request.PaymentMethod);
        await repository.AddAsync(payment);

        // TODO Add Payment Webhook

        Enum.TryParse<EPaymentMethod>(payment.PaymentMethod, true, out var method);
        Enum.TryParse<EPaymentStatus>(payment.PaymentStatus, true, out var status);

        return Result<PaymentResponse>.Ok(new PaymentResponse(payment.Id, payment.OrderId, payment.TotalAmount, method, status));
    }

    public async  Task<Result<List<PaymentResponse>>> ListAsync()
    {
        var payments = await repository.GetAllAsync();

        if (payments == null || payments.Count == 0)
            return Result<List<PaymentResponse>>.Fail(ErrorFactory.NotFound("Não existe nenhum pagamento."));

        return Result<List<PaymentResponse>>.Ok(
            payments.Select(x => new PaymentResponse(
                x.Id,
                x.OrderId,
                x.TotalAmount,
                Enum.Parse<EPaymentMethod>(x.PaymentMethod, true),
                Enum.Parse<EPaymentStatus>(x.PaymentStatus, true)
            )).ToList()
        );
    }
}
