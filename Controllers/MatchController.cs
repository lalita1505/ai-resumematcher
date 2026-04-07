using AI.ResumeMatcher.Models;
using AI.ResumeMatcher.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AI.ResumeMatcher.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchingService _matchingService;

        public MatchController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        public IActionResult Index()
        {
            return View(new MatchViewModel
            {
                Request = new MatchRequest()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Analyze(MatchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var result = await _matchingService.MatchAsync(model.Request);

            model.Result = result;

            return View("Index", model);
        }
    }
}
