using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Player
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstname")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastname")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("shortname")]
        public string ShortName { get; set; } = string.Empty;


        [JsonPropertyName("sex")]
        public string Sex { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public Country Country { get; set; } = new Country();

        [JsonPropertyName("picture")]
        public string Picture { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public PlayerData PlayerData { get; set; } = new PlayerData();
    }
}