using Domain.Entities;
using System.Text.Json.Serialization;

namespace Domain
{
    public class PlayersRoot
    {
        [JsonPropertyName("players")]
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
