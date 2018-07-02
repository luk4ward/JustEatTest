using System;

namespace JustEatTest.Domain.Infrastructure
{
    public class JustEatApiOptions
    {
        public string RestaurantsUri { get; set; }
        public int RequestTimeout { get; set; } = 5000;
        public string Authorization { get; set; }
        public string Host { get; set; }
        public string Language { get; set; }
        public string Tenant { get; set; }
    }
}