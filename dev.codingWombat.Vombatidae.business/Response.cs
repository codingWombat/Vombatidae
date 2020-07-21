using System.Text.Json;

namespace dev.codingWombat.Vombatidae.business
{
    public class Response
    {
        public int StatusCode { get; set; }
        public JsonElement ResponseMessage { get; set; }
    }
}