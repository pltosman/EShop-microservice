using Payment.Domain.Repositories;
using Payment.Infrastructure.Data;
using Payment.Infrastructure.Repositories.Base;

namespace Payment.Infrastructure.Repositories
{
    public class PaymentRepository : Repository<Domain.Entities.Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentContext dbContext) : base(dbContext)
        {

        }
    }
}
