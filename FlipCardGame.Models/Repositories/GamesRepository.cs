using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlipCardGame.Models.Repositories
{
    public class GamesRepository : Repositories.RepositoryBase
    {
        public DAL.Games Get(int id) {
            var res = this.Context.Games.FirstOrDefault(w => w.IDGame == id);
            return res;
        }
    }
}
