using JuggleWebApp.Data.Enum;
using JuggleWebApp.Interfaces;
using JuggleWebApp.Models;
using JuggleWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Linq;

namespace JuggleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ICommentRepository _commentRepository;

        public HomeController(IPostRepository postRepository, ITournamentRepository tournamentRepository, IHttpContextAccessor httpContextAccessor, IBlobStorageService blobStorageService, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _tournamentRepository = tournamentRepository;
            _httpContextAccessor = httpContextAccessor;
            _blobStorageService = blobStorageService;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetAllPosts();
            var sortedPosts = posts.OrderByDescending(p => p.Date);
            return View(sortedPosts);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var post = await _postRepository.GetPostById(id);
            var comments = await _commentRepository.GetCommentsByPostId(id);
            
            foreach(Comment comment in comments)
            {
                if(comment.AppUser.Image == null)
                {
                    comment.AppUser.Image = "https://jugglewebappstorage.blob.core.windows.net/defaults/obraz_2023-04-28_234911255.png";
                }
            }

            var sortedComments = comments.OrderByDescending(c => c.Date);

            var detailPostViewModel = new DetailPostViewModel()
            {
                Post = post,
                Comment = new Comment(),
                CommentsList = sortedComments,
            };


            return View(detailPostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(DetailPostViewModel detailPostViewModel)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var comment = new Comment()
            {
                AppUserId = userId,
                PostId = detailPostViewModel.Post.Id,
                Text = detailPostViewModel.Comment.Text,
                Date = DateTime.Now,
            };
            _commentRepository.Add(comment);
            return RedirectToAction("Detail", new { id = detailPostViewModel.Post.Id });
        }

        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            var comment = await _commentRepository.GetCommentById(commentId);

            if (comment != null)
            {
                _commentRepository.Delete(comment);
            }
            return RedirectToAction("Detail", new { id = postId });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tournaments = await _tournamentRepository.GetAllTournaments();
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var createPostViewModel = new CreatePostViewModel()
            {
                TournamentsList = Converters.TournamentListToItemListConverter(tournaments),
                AppUserId = userId,
            };
            return View(createPostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel createPostViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to create post");
                return View(createPostViewModel);
            }

            string imageAddress = await _blobStorageService.AddFile(createPostViewModel.Image, "files");


            var post = new Post()
            {
                Title = createPostViewModel.Title,
                Description = createPostViewModel.Description,
                //Image = createPostViewModel.Image,
                Image = imageAddress,

                TournamentId = createPostViewModel.SelectedTournamentId,

                AppUserId = createPostViewModel.AppUserId,
                AppUser = createPostViewModel.AppUser,

                Date = DateTime.Now,
                LikesNumber = 0,
                Points = 0,

            };

            var result = _postRepository.Add(post);
            if (!result)
            {
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postRepository.GetPostById(id);
            var tournaments = await _tournamentRepository.GetAllTournaments();

            var editPostViewModel = new EditPostViewModel
            {
                Id = id,
                Title = post.Title,
                Description = post.Description,
                PreviousImage = post.Image,

                SelectedTournamentId = post.TournamentId,
                TournamentsList = Converters.TournamentListToItemListConverter(tournaments),

                AppUserId = post.AppUserId,
                AppUser = post.AppUser,

                LikesNumber = post.LikesNumber,
                Points = post.Points,
            };

            return View(editPostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditPostViewModel editPostViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit post");
                return View("Edit", editPostViewModel);
            }
            string imageAddress;
            if (editPostViewModel.NewImage != null)
            {
                imageAddress = await _blobStorageService.AddFile(editPostViewModel.NewImage, "files");
                if (imageAddress != null)
                {
                    var deleteResult = await _blobStorageService.DeleteFile(editPostViewModel.PreviousImage, "files");
                }
            }
            else
            {
                imageAddress = editPostViewModel.PreviousImage;
            }


            var post = new Post()
            {
                Id = editPostViewModel.Id,
                Title = editPostViewModel.Title,
                Description = editPostViewModel.Description,
                Image = imageAddress,

                TournamentId = editPostViewModel.SelectedTournamentId,

                AppUserId = editPostViewModel.AppUserId,
                AppUser = editPostViewModel.AppUser,

                Date = DateTime.Now,

                LikesNumber = (int)editPostViewModel.LikesNumber,
                Points = (int)editPostViewModel.Points,
            };

            _postRepository.Update(post);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postRepository.GetPostById(id);

            if (post != null)
            {
                //delete post's comments
                var comments = await _commentRepository.GetCommentsByPostId(post.Id);
                if (comments.Count() > 0)
                {
                    foreach (var comment in comments)
                    {
                        _commentRepository.Delete(comment);
                    }
                }


                //delete post's image from Azure
                var blobResult = await _blobStorageService.DeleteFile(post.Image, "files");
                //delete post from database
                var postResult = _postRepository.Delete(post);
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
