using FCG.Payments.Domain.Enums;
using FCG.Payments.Domain.Interfaces.Repositories;

namespace FCG.Payments.Domain.Entities;

public class PaymentEntity : EntityBase, IAggregateRoot
{
    public Guid OrderId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string PaymentMethod { get; private set; }
    public string PaymentStatus { get; private set; }


    public PaymentEntity() { }

    public PaymentEntity(Guid orderId, decimal totalAmount, EPaymentMethod paymentMethod)
    {
        OrderId = orderId;
        TotalAmount = totalAmount;
        PaymentMethod = paymentMethod.ToString();
        PaymentStatus = EPaymentStatus.InProcess.ToString();
    }

    public void ChangeStatus(EPaymentStatus status)
    {
        PaymentStatus = status.ToString();
    }
}
