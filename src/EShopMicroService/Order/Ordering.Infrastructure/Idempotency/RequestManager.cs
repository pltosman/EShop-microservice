using System;
using System.Threading.Tasks;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Exceptions;

namespace Ordering.Domain.Idempotency
{
  public class RequestManager : IRequestManager
    {
        private readonly OrderContext _context;

        public RequestManager(OrderContext context)
        {
            _context = context;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new OrderException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.FindAsync<ClientRequest>(id);

            return request != null;
        }
    }
}