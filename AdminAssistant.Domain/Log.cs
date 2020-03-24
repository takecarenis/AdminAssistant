using System;

namespace AdminAssistant.Domain
{
    public class Log : IEntity
    {
        public int Id { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }

    public enum LogType
    {
        Information,
        Error
    }
}
