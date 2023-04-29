using JuggleWebApp.Interfaces;
using JuggleWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JuggleWebApp.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IPostRepository _postRepository;

        public TournamentController(ITournamentRepository tournamentRepository, IPostRepository postRepository)
        {
            _tournamentRepository = tournamentRepository;
            _postRepository = postRepository;
        }


        public async Task<IActionResult> Index()
        {
            var tournaments = await _tournamentRepository.GetAllTournaments();

            var tournamentViewModelList = new List<TournamentViewModel>();
            
            foreach(var tournament in tournaments)
            {
                var posts = await _postRepository.GetPostsByTournamentId(tournament.Id);
                tournamentViewModelList.Add(new TournamentViewModel
                {
                    Id = tournament.Id,
                    Title = tournament.Title,
                    Description = tournament.Description,
                    Posts = posts,
                });
            }

            return View(tournamentViewModelList);
        }
    }
}
