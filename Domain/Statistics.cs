using Domain.Entities;

namespace Domain
{
    public class Statistics
    {
        public Country? CountryWithBestWinningRatio { get; set; }
        public double AveragePlayersIMC { get; set; }
        public double PlayersHeightMedian { get; set; }
    }
}
