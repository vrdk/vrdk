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
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly HRMSystemDbContext _context;

        public TransactionTypeRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TransactionType entity)
        {
            _context.TransactionType.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TransactionType entity)
        {
            _context.TransactionType.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<TransactionType[]> GetAsync(Expression<Func<TransactionType, bool>> condition = null)
        {
            return condition != null ? await _context.TransactionType.Where(condition).ToArrayAsync() : await _context.TransactionType.ToArrayAsync();
        }

        public async Task<TransactionType> GetByIdAsync(string id)
        {
            return await _context.TransactionType.FirstOrDefaultAsync(ab => ab.TransactionTypeId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
