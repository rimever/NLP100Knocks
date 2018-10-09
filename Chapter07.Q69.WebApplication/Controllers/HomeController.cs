#region

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Chapter07.Core;
using Chapter07.Core.Models;
using Chapter07.Q69.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

#endregion

namespace Chapter07.Q69.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly AnswerService
            _service = new AnswerService(new ConnectionString(@"..\..\..\..\Chapter07.Core"));

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Search(string keyword, bool isArtist, bool isAlias, bool isTags)
        {
            var results = _service.Answer69(keyword, isArtist, isAlias, isTags);
            return PartialView(new SearchViewModel(results));
        }
    }

    /// <summary>
    /// 検索結果ViewModel
    /// </summary>
    public class SearchViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="results"></param>
        public SearchViewModel(IList<string> results)
        {
            Results = EnumerableArtists(results).ToList();
        }

        public IList<Artist> Results { get; set; }

        private IEnumerable<Artist> EnumerableArtists(IList<string> results)
        {
            foreach (string result in results)
            {
                yield return new Artist(JObject.Parse(result));
            }
        }
    }
}