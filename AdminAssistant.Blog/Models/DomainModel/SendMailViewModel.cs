using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Models.DomainModel
{
    public class SendMailViewModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Users { get; set; }
    }
}
