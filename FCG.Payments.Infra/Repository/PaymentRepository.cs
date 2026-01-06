using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Interfaces.Repositories;
using FCG.Payments.Infra.Contexts;

namespace FCG.Payments.Infra.Repository;

public class PaymentRepository : RepositoryBase<PaymentEntity>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }
}