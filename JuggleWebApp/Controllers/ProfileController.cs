using JuggleWebApp.Interfaces;
using JuggleWebApp.Models;
using JuggleWebApp.Repositories;
using JuggleWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace JuggleWebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IBlobStorageService _blobStorageService;

        public ProfileController(UserManager<AppUser> userManager, IPostRepository postRepository, ICommentRepository commentRepository, IBlobStorageService blobStorageService)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> MyProfile(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var posts = await _postRepository.GetPostsByUser(user);
            var comments = await _commentRepository.GetCommentsByUser(user);

            var sortedPosts = posts.OrderByDescending(p => p.Date);
            var sortedComments = comments.OrderByDescending(c => c.Date);

            if (user != null)
            {
                if (user.Image == null) user.Image = "https://jugglewebappstorage.blob.core.windows.net/defaults/obraz_2023-04-28_234911255.png";
                var profileViewModel = new ProfileViewModel
                {
                    UserName = userName,
                    Name = user.Name,
                    Description = user.Description,
                    Image = user.Image,
                    Posts = sortedPosts,
                    Comments = sortedComments,
                };
                return View(profileViewModel);
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Edit(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var editProfileViewModel = new EditProfileViewModel()
                {
                    Name = user.Name,
                    Description = user.Description,
                    ImageURL = user.Image,
                    UserName = userName,
                };
                return View(editProfileViewModel);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel editProfileViewModel)
        {
            var user = await _userManager.FindByNameAsync(editProfileViewModel.UserName);
            string imageURL;
            if (editProfileViewModel.Image != null)
            {
                if (editProfileViewModel.ImageURL != null)
                {
                    await _blobStorageService.DeleteFile(editProfileViewModel.ImageURL, "profiles");
                }
                imageURL = await _blobStorageService.AddFile(editProfileViewModel.Image, "profiles");
            }
            else
            {
                imageURL = editProfileViewModel.ImageURL;
            }

            if (user != null)
            {
                var editedUser = user;

                editedUser.Name = editProfileViewModel.Name;
                editedUser.Image = imageURL;
                editedUser.Description = editProfileViewModel.Description;

                await _userManager.UpdateAsync(editedUser);

            }

            return RedirectToAction("MyProfile", new { userName = editProfileViewModel.UserName });
        }
    }
}
