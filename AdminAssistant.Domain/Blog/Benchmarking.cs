using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAssistant.Domain.Blog
{
    public class Benchmarking : IEntity
    {
        public int Id { get; set; }
        public string UniqueUrl { get; set; }
        public DateTime Created { get; set; }
        public bool isActive { get; set; }
        public List<BenchmarkingQuestion> Questions { get; set; }
    }
}
