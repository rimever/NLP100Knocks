#region

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Chapter06.Core;
using Chapter06.Core.Models;
using Chapter06.Q57.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Chapter06.Q57.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IList<Sentence> _sentences;

        private IList<Sentence> Sentences
        {
            get
            {
                if (_sentences == null)
                {
                    var answerService = new AnswerService(@"..\..\..\..\..\Chapter06.Core");
                    _sentences = answerService.Answer57().ToList();
                }

                return _sentences;
            }
        }


        public IActionResult Index()
        {
            var sentences = Sentences;

            return View(new TextViewModel(sentences));
        }

        /// <summary>
        /// 文章の係り受け情報を取得します。
        /// </summary>
        /// <remarks>
        /// ex.http://localhost:5000/Home/SentenceLink?sentenceIndex=1
        /// </remarks>
        /// <param name="sentenceIndex">文章のインデックス</param>
        /// <returns></returns>
        public JsonResult SentenceLink(int sentenceIndex)
        {
            var sentence = Sentences[sentenceIndex];
            var nodes = sentence.Words.Select(s => new {label = s.Value}).ToList();
            var dependencies = sentence.DependencyDictionary["collapsed-dependencies"];
            var links = dependencies.Where(d => d.Governor.Index > 0).Select(d => new
                {target = d.Governor.Index - 1, source = d.Dependent.Index - 1});

            return new JsonResult(new Dictionary<string, object>
            {
                {"nodes", nodes},
                {"links", links}
            });
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
    }
}