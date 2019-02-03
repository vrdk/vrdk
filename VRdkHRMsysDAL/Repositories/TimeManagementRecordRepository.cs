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
    public class TimeManagementRecordRepository : ITimeManagementRecordRepository
    {
        private readonly HRMSystemDbContext _context;

        public TimeManagementRecordRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TimeManagementRecord entity, bool writeChanges)
        {
            _context.TimeManagementRecord.Add(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(TimeManagementRecord entity, bool writeChanges)
        {
            _context.TimeManagementRecord.Remove(_context.TimeManagementRecord.FirstOrDefault(tm => tm.TimeManagementRecordId == entity.TimeManagementRecordId));

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task<TimeManagementRecord[]> GetAsync(Expression<Func<TimeManagementRecord, bool>> condition = null)
        {
            return condition != null ? await _context.TimeManagementRecord.Where(condition).ToArrayAsync() : await _context.TimeManagementRecord.ToArrayAsync();
        }

        public async Task<TimeManagementRecord> GetByIdAsync(string id)
        {
            return await _context.TimeManagementRecord.FirstOrDefaultAsync(tm => tm.TimeManagementRecordId.Equals(id));
        }

        public async Task UpdateAsync(TimeManagementRecord entity, bool writeChanges)
        {
            _context.TimeManagementRecord.Update(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task WriteChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
