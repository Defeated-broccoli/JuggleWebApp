using JuggleWebApp.Data;
using JuggleWebApp.Interfaces;
using JuggleWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JuggleWebApp.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Post post)
        {
            _context.Add(post);
            return Save();
        }

        public bool Delete(Post post)
        {
            _context.Remove(post);
            return Save();
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _context.Posts.Include(p => p.AppUser).ToListAsync();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _context.Posts.Include(p => p.Tournament).Include
                (p => p.AppUser).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsByTournamentId(int id)
        {
            return await _context.Posts.Where(p => p.TournamentId == id).ToListAsync();
        }

        public async Task<List<Post>> GetPostsByUser(AppUser user)
        {
            return await _context.Posts.Where(p => p.AppUser == user).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;    
        }

        public bool Update(Post post)
        {
            _context.Update(post);
            return Save();
        }
    }
}
