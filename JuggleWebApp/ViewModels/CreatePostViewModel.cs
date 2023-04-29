using JuggleWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JuggleWebApp.ViewModels
{
    public class CreatePostViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public int? SelectedTournamentId { get; set; }
        public List<SelectListItem> TournamentsList { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

    }

}
