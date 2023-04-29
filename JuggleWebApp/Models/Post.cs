using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuggleWebApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [ForeignKey("Tournament")]
        public int? TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public DateTime Date { get; set; }
        public int LikesNumber { get; set; }
        public int Points { get; set; }

    }
}
