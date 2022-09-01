﻿using System;
using Chat.Application.Interfaces;

namespace Chat.Application
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
