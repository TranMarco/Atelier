using Application.Interfaces;
using Domain;
using Domain.Entities;

namespace Application
{
    public class PlayersRetriever : IPlayersRetriever
    {
        private readonly Lazy<PlayersRoot> _lazyPlayersRoot;

        public PlayersRetriever(IPlayersRepository playersRepository)
        {
            _lazyPlayersRoot = new Lazy<PlayersRoot>(() => playersRepository.GetPlayersRoot());
        }

        public List<Player> GetSortedPlayersFromBestToWorst()
        {
            return _lazyPlayersRoot.Value.Players.OrderBy(player => player.PlayerData.Rank).ToList();
        }

        public Player GetPlayerFromId(int id)
        {
            return _lazyPlayersRoot.Value.Players.First(player => player.Id == id);
        }

        public Statistics CalculateStatistics()
        {
            var players = _lazyPlayersRoot.Value.Players;
            Country bestCountry = GetCountryWithBestWinningRatio(players);
            var averageIMCOfAllPlayers = CalculateAverageIMCOfPlayers(players);
            var playersHeightMedian = CalculatePlayersHeightMedian(players);

            return new Statistics
            {
                CountryWithBestWinningRatio = bestCountry,
                AveragePlayersIMC = averageIMCOfAllPlayers,
                PlayersHeightMedian = playersHeightMedian
            };
        }

        private double CalculatePlayersHeightMedian(List<Player> players)
        {
            var orderedHeights = players.Select(player => player.PlayerData.Height).OrderBy(height => height).ToList();
            if (orderedHeights.Count % 2 == 1)
            {
                return orderedHeights[orderedHeights.Count / 2 + 1];
            }
            else
            {
                var firstMiddlePosition = orderedHeights.Count / 2 - 1;
                return (orderedHeights[firstMiddlePosition] + orderedHeights[firstMiddlePosition + 1]) / 2d;
            }
        }

        private static double CalculateAverageIMCOfPlayers(List<Player> players)
        {
            var averageIMC = players
                .Select(player => IMCFormula(player))
                .Average();
            return averageIMC;
        }

        private static double IMCFormula(Player player)
        {
            var imc = (player.PlayerData.Weight / 1000d) / Math.Pow(player.PlayerData.Height / 100d, 2);
            return imc;
        }

        private static Country GetCountryWithBestWinningRatio(List<Player> players)
        {
            var bestCountryKey = players
                .GroupBy(player => player.Country.Code)
                .MaxBy(group => group
                    .SelectMany(player => player.PlayerData.Last)
                    .DefaultIfEmpty()
                    .Average());
            var bestCountry = players.Select(player => player.Country).First(country => country.Code == bestCountryKey!.Key);
            return bestCountry;
        }
    }
}
