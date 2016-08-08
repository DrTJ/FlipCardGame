using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlipCardGame.Controllers
{
    public class GameController : BaseController
    {
        Models.Repositories.GamesRepository repoGames;

        public GameController() : base() {
            this.repoGames = new Models.Repositories.GamesRepository();
        }

        public ActionResult Index() {

            return View();
        }

        public ActionResult Play(int id) {
            // read the game[id]'s data 
            var game = new DAL.Games();  // repoGames.Get(id);
            
            return View(game);
        }
    }
}
