using AdminAssistant.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Interfaces
{
    public interface ILogService
    {
        void Log(LogType type, string message);
    }
}
