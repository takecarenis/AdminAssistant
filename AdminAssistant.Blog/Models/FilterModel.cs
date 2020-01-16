using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Models
{
    public enum SortType
    {
        Asc = 1,
        Desc = 2
    }

    public class FilterModel
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public SortType Sort { get; set; }
    }
}
