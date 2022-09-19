using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Country
    {

        [JsonPropertyName("picture")]
        public string Picture { get; set; } = string.Empty;


        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
    }
}
