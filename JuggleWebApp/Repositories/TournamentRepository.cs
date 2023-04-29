using JuggleWebApp.Data;
using JuggleWebApp.Interfaces;
using JuggleWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JuggleWebApp.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly ApplicationDbContext _context;

        public TournamentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Tournament tournament)
        {
            _context.Add(tournament);
            return Save();
        }

        public bool Delete(Tournament tournament)
        {
            _context.Remove(tournament);
            return Save();
        }

        public async Task<List<Tournament>> GetAllTournaments()
        {
            return await _context.Tournaments.Include(t => t.Posts).ToListAsync();
        }

        public async Task<Tournament> GetTournamentById(int id)
        {
            return await _context.Tournaments.Include(t => t.Posts).FirstOrDefaultAsync(t => t.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Tournament tournament)
        {
            _context.Update(tournament);
            return Save();  
        }
    }
}
