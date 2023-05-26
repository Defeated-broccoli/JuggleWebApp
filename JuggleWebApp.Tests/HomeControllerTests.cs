using FakeItEasy;
using JuggleWebApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public HomeControllerTests()
        {
            //Dependencies
            _postRepository = A.Fake<IPostRepository>();
            _tournamentRepository = A.Fake<ITournamentRepository>();
            _commentRepository = A.Fake<ICommentRepository>();

            _blobStorageService = A.Fake<IBlobStorageService>();
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();
        }
    }
}
