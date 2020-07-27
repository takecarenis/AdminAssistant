namespace AdminAssistant.Domain.Blog
{
    public class BenchmarkingQuestion : IEntity
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int Type { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }

        public string Answer3 { get; set; }

        public string Answer4 { get; set; }

        public string Answer5 { get; set; }

        public string Answer6 { get; set; }
        public string Answer7 { get; set; }
    }
}