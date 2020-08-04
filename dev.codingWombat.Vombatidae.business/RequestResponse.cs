using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace dev.codingWombat.Vombatidae.business
{
    public class RequestResponse
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string HttpMethod { get; set; }
        public JsonElement RequestBody { get; set; }
        public Response ResponseBody { get; set; }
        public List<KeyValuePair<string, StringValues>> QueryParams { get; set; }
        public string Route { get; set; }
    }
}