using AdminAssist.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAssist.Domain
{
    public class NewsletterHistory : IEntity
    {
        public int Id { get; set; }
        public Newsletter Newsletter { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
    }
}
