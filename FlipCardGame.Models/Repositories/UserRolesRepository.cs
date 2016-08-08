using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlipCardGame.Models.Repositories
{
    public class UserRolesRepository : RepositoryBase, IRepositoryBase<DAL.FlipCardGameEntities>
    {
        public UserRolesRepository() : base() { }
    }
}
