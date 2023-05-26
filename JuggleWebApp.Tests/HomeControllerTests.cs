using FakeItEasy;
using FluentAssertions;
using JuggleWebApp.Controllers;
using JuggleWebApp.Interfaces;
using JuggleWebApp.Models;
using JuggleWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JuggleWebApp.Tests
{
    public class HomeControllerTests
    {
        private IPostRepository _postRepository;
        private ITournamentRepository _tournamentRepository;
        private ICommentRepository _commentRepository;

        private IBlobStorageService _blobStorageService;
        private IHttpContextAccessor _httpContextAccessor;

        private HomeController _homeController;

        public HomeControllerTests()
        {
            //Dependencies
            _postRepository = A.Fake<IPostRepository>();
            _tournamentRepository = A.Fake<ITournamentRepository>();
            _commentRepository = A.Fake<ICommentRepository>();

            _blobStorageService = A.Fake<IBlobStorageService>();
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();

            //SUT
            _homeController = new HomeController(_postRepository, _tournamentRepository, _commentRepository, _httpContextAccessor, _blobStorageService);
        }

        [Fact]
        public async Task HomeController_Index_ReturnsSuccess()
        {
            // Arrange
            var posts = A.Fake<List<Post>>();
            A.CallTo(() => _postRepository.GetAllPosts()).Returns(posts);

            // Act
            var result = await _homeController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task HomeController_Detail_ReturnsViewWithViewModel()
        {
            // Arrange
            int postId = 1;
            var expectedPost = new Post { Id = postId };
            var expectedComments = new List<Comment>
            {
                new Comment { Id = 1, Text = "test 1", Date = new DateTime(2023, 5, 1) },
                new Comment { Id = 2, Text = "test 2", Date = new DateTime(2023, 5, 3) },
                new Comment { Id = 3, Text = "test 3", Date = new DateTime(2023, 5, 2) }
            };

            A.CallTo(() => _postRepository.GetPostById(postId)).Returns(expectedPost);
            A.CallTo(() => _commentRepository.GetCommentsByPostId(postId)).Returns(expectedComments);

            var expectedViewModel = new DetailPostViewModel
            {
                Post = expectedPost,
                Comment = new Comment(),
                CommentsList = expectedComments
            };

            // Act
            var result = await _homeController.Detail(postId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeOfType<DetailPostViewModel>();
            var viewModel = viewResult.Model as DetailPostViewModel;
            viewModel.Should().BeEquivalentTo(expectedViewModel);
        }

        [Fact]
        public async void HomeController_AddComment_ReturnsSuccess()
        {
            int postId = 1;
            var viewModelPost = new Post { Id = postId };

            string userId = "userid";
            string commentText = "texttext";

            //Arange
            var detailPostViewModel = new DetailPostViewModel()
            {
                Post = viewModelPost,
                Comment = new Comment()
                {
                    Text = commentText
                },
            };

            var comment = new Comment()
            {
                AppUserId = userId,
                PostId = postId,
                Text = detailPostViewModel.Comment.Text,
                Date = DateTime.Now,
            };

            A.CallTo(() => _commentRepository.Add(comment)).Returns(true);

            //Act
            var result = await _homeController.AddComment(detailPostViewModel);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteValues["id"].Should().Be(postId);
        }
    }
}
