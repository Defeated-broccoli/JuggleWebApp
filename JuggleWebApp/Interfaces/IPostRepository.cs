using JuggleWebApp.Models;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace JuggleWebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPosts();
        Task<Post> GetPostById(int id);
        Task<List<Post>> GetPostsByTournamentId(int id);
        Task<List<Post>> GetPostsByUser (AppUser user);

        bool Add(Post post);
        bool Update(Post post);
        bool Delete(Post post);
        bool Save();

    }
}
