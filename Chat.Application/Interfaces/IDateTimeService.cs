using System;

namespace Chat.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
