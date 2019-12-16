using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAssistant.Domain
{
    public class Newsletter : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
