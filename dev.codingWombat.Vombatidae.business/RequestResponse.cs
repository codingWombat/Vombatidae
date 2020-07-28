using System;
using System.Text.Json;

namespace dev.codingWombat.Vombatidae.business
{
    public class RequestResponse
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string HttpMethod { get; set; }
        public JsonElement RequestBody { get; set; }
        public Response ResponseBody { get; set; }
    }
}