﻿﻿//-----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Processa"> 
// Copyright (c) 2016 Todos los derechos reservados.
// </copyright>
// <author>mtaghi</author>
// <date>3/31/2016 8:00:00 AM</date>
//-----------------------------------------------------------------------
namespace FlipCardGame.Controllers
{
    #region using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    #endregion

    [Filters.InitializeSimpleMembership()]
    public class BaseController : Controller
    {
    }
}