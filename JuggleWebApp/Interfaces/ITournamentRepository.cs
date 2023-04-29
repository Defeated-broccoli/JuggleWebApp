using JuggleWebApp.Models;

namespace JuggleWebApp.Interfaces
{
    public interface ITournamentRepository
    {
        Task<List<Tournament>> GetAllTournaments();
        Task<Tournament> GetTournamentById(int id);

        bool Add(Tournament tournament);
        bool Update(Tournament tournament);
        bool Delete(Tournament tournament);
        bool Save();
    }
}
