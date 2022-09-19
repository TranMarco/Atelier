using Domain;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPlayersRetriever
    {
        List<Player> GetSortedPlayersFromBestToWorst();
        Player GetPlayerFromId(int id);
        Statistics CalculateStatistics();
    }
}
