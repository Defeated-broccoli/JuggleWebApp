using JuggleWebApp.Models;

namespace JuggleWebApp.ViewModels
{
    public class ProfileViewModel
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public IOrderedEnumerable<Comment>? Comments { get; set; }
        public IOrderedEnumerable<Post>? Posts { get; set; }
    }
}
