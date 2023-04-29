namespace JuggleWebApp.ViewModels
{
    public class EditProfileViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }

        public string ImageURL { get; set; }
        public string? UserName { get; set; }
    }
}
