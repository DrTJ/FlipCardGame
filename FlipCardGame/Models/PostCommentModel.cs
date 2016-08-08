using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlipCardGame.Models
{
    public class PostCommentModel
    {
        public string FullName { get; set; }
        public string CommentBody { get; set; }
        public DateTime CommentDateTime { get; set; }
    }
}