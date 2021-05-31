using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly OrderContext _context;

        public IOrderRepository Orders { get; }

        public UnitOfWork(OrderContext context,
                 IOrderRepository orderRepository)
        {
            _context = context;
            Orders = orderRepository;

        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}