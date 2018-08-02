using System.Collections.Generic;
using System.Web.Mvc;
using WebjetCodeChallenge.DataAccess;
using WebjetCodeChallenge.Models;

namespace WebjetCodeChallenge.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, MovieDetails> _movieDatabase;

        public HomeController()
        {
            if (_movieDatabase == null)
                _movieDatabase = new Dictionary<string, MovieDetails>();

        }
        public ActionResult Index()
        {
            //load data
            MovieListAccess.UpdateMovieDatabase();
            _movieDatabase = MovieListAccess.MovieDatabase;

            return View(_movieDatabase);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Details(string movieName = "Blank")
        {
            return View(_movieDatabase[movieName]);
        }
    }
}