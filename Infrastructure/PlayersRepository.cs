using Application.Interfaces;
using Domain;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure
{
    public class PlayersRepository : IPlayersRepository
    {
        public PlayersRoot GetPlayersRoot()
        {
            using (StreamReader file = File.OpenText($@"{Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)}\headtohead.json"))
            {
                var result = JsonSerializer.Deserialize<PlayersRoot>(file.BaseStream);
                return result ?? throw new ArgumentNullException(nameof(result));
            }
        }
    }
}
