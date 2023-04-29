using JuggleWebApp.Data;
using JuggleWebApp.Interfaces;
using JuggleWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JuggleWebApp.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Comment comment)
        {
            _context.Comments.Add(comment);
            return Save();
        }

        public bool Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            return Save();
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetCommentsByPostId(int id)
        {
            return await _context.Comments.Include(c => c.AppUser).Include(c => c.Post).Where(c => c.PostId == id).ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByUserId(string id)
        {
            return await _context.Comments.Include(c => c.AppUser).Include(c => c.Post).Where(c => c.AppUserId == id).ToListAsync(); 
        }

        public async Task<List<Comment>> GetCommentsByUser(AppUser user)
        {
            return await _context.Comments.Include(c => c.AppUser).Include(c => c.Post).Where(c => c.AppUser == user).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Comment comment)
        {
            _context.Comments.Update(comment);
            return Save();
        }
    }
}
