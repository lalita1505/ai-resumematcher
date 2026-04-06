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
            return View();
        }

        //[HttpPost("analyze")]
        //public async Task<IActionResult> Analyze(MatchRequest request)
        //{
        //    string resumeText;

        //    using (var reader = new StreamReader(request.ResumeFile.OpenReadStream()))
        //    {
        //        resumeText = await reader.ReadToEndAsync();
        //    }

        //    var result = await _matchingService.ProcessAsync(
        //        request.JobDescription,
        //        resumeText);

        //    return View("Result", result);
        //}

        //[HttpPost("match")]
        //public async Task<IActionResult> OpenAIAnalyze(MatchRequest request)
        //{
        //    var result = await _matchingService.ProcessMatchAsync(request);
        //    return View("Result", result);
        //}


        [HttpPost]
        public async Task<IActionResult> Analyze(MatchRequest request)
        {
            var result = await _matchingService.MatchAsync(request);

            //ViewBag.Result = result;
            return View("Index", result);

        }
    }
}
