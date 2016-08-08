using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlipCardGame.Models.Repositories
{
    public interface IRepositoryBase<ContextType>
    {
        ContextType Context { get; set; }
    }
}
