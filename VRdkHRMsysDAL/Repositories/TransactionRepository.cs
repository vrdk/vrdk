using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Contexts;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysDAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly HRMSystemDbContext _context;

        public TransactionRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Transaction entity, bool writeChanges)
        {
            _context.Transaction.Add(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(Transaction entity, bool writeChanges)
        {
            _context.Transaction.Remove(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task<Transaction[]> GetAsync(Expression<Func<Transaction, bool>> condition = null)
        {
            return condition != null ? await _context.Transaction.Where(condition).ToArrayAsync() : await _context.Transaction.ToArrayAsync();
        }

        public async Task<Transaction> GetByIdAsync(string id)
        {
            return await _context.Transaction.FirstOrDefaultAsync(ab => ab.TransactionId.Equals(id));
        }

        public async Task WriteChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
