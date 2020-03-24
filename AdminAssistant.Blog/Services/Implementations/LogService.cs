using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Implementations
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _dbContext;

        public LogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Log(LogType type, string message)
        {
            _dbContext.Logs.Add(new Log
            {
                Type = type,
                Date = DateTime.Now,
                Message = message
            });

            _dbContext.SaveChanges();
        }
    }
}
