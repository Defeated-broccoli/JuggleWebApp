using JuggleWebApp.Models;

namespace JuggleWebApp.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentById(int id);
        Task<List<Comment>> GetCommentsByPostId(int id);
        Task<List<Comment>> GetCommentsByUserId(string id);
        Task<List<Comment>> GetCommentsByUser(AppUser user);
        bool Add(Comment comment);
        bool Update(Comment comment);
        bool Delete(Comment comment);
        bool Save();
    }
}
