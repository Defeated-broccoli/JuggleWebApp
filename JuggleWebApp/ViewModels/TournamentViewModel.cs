using JuggleWebApp.Models;

namespace JuggleWebApp.ViewModels
{
    public class TournamentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
