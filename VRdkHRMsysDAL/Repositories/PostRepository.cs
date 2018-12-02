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
    public class PostRepository : IPostRepository
    {
        private readonly HRMSystemDbContext _context;

        public PostRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Post entity)
        {
            _context.Post.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post entity)
        {
            _context.Post.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Post[]> GetAsync(Expression<Func<Post, bool>> condition = null)
        {
            return condition != null ? await _context.Post.Where(condition).ToArrayAsync() : await _context.Post.ToArrayAsync();
        }

        public async Task<Post> GetByIdAsync(string id)
        {
            return await _context.Post.FirstOrDefaultAsync(ab => ab.PostId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
