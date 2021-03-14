using System;

namespace Core.Dtos.Settings
{
    public class MemoryCacheSettings
    {
        public TimeSpan MenuExpiration { get; set; }
        public TimeSpan PageExpiration { get; set; }
        public TimeSpan ContactInfoExpiration { get; set; }
    }
}
