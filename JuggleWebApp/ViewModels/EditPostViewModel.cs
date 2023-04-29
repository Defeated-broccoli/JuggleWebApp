using JuggleWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JuggleWebApp.ViewModels
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? PreviousImage { get; set; }
        public IFormFile? NewImage { get; set; }
        public int? SelectedTournamentId { get; set; }
        public List<SelectListItem> TournamentsList { get; set; }

        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public int? LikesNumber { get; set; }
        public int? Points { get; set; }
    }
}
