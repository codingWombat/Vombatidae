using System;

namespace dev.codingWombat.Vombatidae.business
{
    public class RequestResponse
    {
        public DateTime Timestamp { get; set; }
        public string HttpMethod { get; set; }
        public string RequestBody { get; set; }
        public Response ResponseBody { get; set; }
    }
}