﻿
using System;
using System.Threading.Tasks;
using EShop.Core.Infrastructure.Data;
using EShop.Core.Infrastructure.Exceptions;

namespace EShop.Core.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly IdentityContext _context;

        public RequestManager(IdentityContext context)
        {
            _context = context;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new IdentityException($"Request with {id} already exists") :
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
