using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chapter07.Core;
using Microsoft.AspNetCore.Mvc;
using Chapter07.Q69.WebApplication.Models;

namespace Chapter07.Q69.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private AnswerService _service = new AnswerService(new ConnectionString(@"..\..\..\..\Chapter07.Core"));
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string keyword, bool isArtist, bool isAlias, bool isTags)
        {
            var results = _service.Answer69(keyword, isArtist, isAlias, isTags);
            return PartialView(new SearchViewModel(results));
        }
    }

    public class SearchViewModel
    {
        public IList<string> Results { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="results"></param>
        public SearchViewModel(IList<string> results)
        {
            Results = results;
        }
    }
}
