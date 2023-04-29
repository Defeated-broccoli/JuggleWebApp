using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design.Serialization;

namespace JuggleWebApp.Models
{
    public class AppUser :IdentityUser
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Post>? Posts { get; set; }
    }
}
