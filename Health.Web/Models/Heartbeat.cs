using System;

namespace Health.Web.Models
{
    public class Heartbeat
    {
        public long Id { get; set; }

        public string DeviceId { get; set; }

        public DateTime Timestamp { get; set; }

        public int Value { get; set; }
    }
}