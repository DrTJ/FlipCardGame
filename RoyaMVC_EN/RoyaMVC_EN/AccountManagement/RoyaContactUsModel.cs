using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RoyaMVC_EN.AccountManagement
{
    public class RoyaContactUsModel
    {
        [Display(Name = "Full name")]
        public string PersonFullName { get; set; }

        [Display(Name = "EMail")]
        public string EMail { get; set; }

        [Display(Name = "Website")]
        public string WebSite { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Subject")]
        public string MessageSubject { get; set; }

        [Display(Name = "Message")]
        public string MessageBody { get; set; }

        public long MessageID { get; set; }
        public bool IsRead { get; set; }
        
        public DateTime MessageDateTime { get; set; }

        public bool RememberMe { get; set; }
    }
}
