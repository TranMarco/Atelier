using Application;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Text.Json;

namespace Tests
{
    public class ApplicationUnitTests
    {
        // Manage random last digit
        private const double NumberCompareThreshold = 0.00000000001;
        private IPlayersRetriever _playersRetriever;

        [SetUp]
        public void Setup()
        {
            _playersRetriever = InitializeDependancyInjectionAndGetPlayersRetriever(JsonSerializer.Deserialize<PlayersRoot>(HardCodedPlayersJsonForUnitTests)!);

        }

        private static IPlayersRetriever InitializeDependancyInjectionAndGetPlayersRetriever(PlayersRoot playersRoot)
        {
            var services = new ServiceCollection(); ;
            var substitutePlayersRepository = Substitute.For<IPlayersRepository>();
            substitutePlayersRepository.GetPlayersRoot().Returns(playersRoot);
            services.AddSingleton(substitutePlayersRepository);
            services.AddSingleton<IPlayersRetriever, PlayersRetriever>();
            var serviceProvider = services.BuildServiceProvider();
            var playersRetriever = serviceProvider.GetService<IPlayersRetriever>() ?? throw new ArgumentNullException();

            return playersRetriever;
        }

        [Test]
        public void Application_Should_Give_Players_Sorted_From_Best_To_Worst()
        {
            var playersSorted = _playersRetriever.GetSortedPlayersFromBestToWorst();
            var result = string.Join(',', playersSorted.Select(player => player.PlayerData.Rank));
            Assert.That(result, Is.EqualTo("1,2,10,21,52"));
        }

        [TestCase(52, "N.DJO")]
        [TestCase(95, "V.WIL")]
        [TestCase(65, "S.WAW")]
        [TestCase(102, "S.WIL")]
        [TestCase(17, "R.NAD")]
        public void Application_Should_Get_Right_Player_From_Id(int id, string expectedShortName)
        {
            var player = _playersRetriever.GetPlayerFromId(id);
            Assert.That(player.ShortName, Is.EqualTo(expectedShortName));
        }

        [Test]
        public void Application_Should_Get_Best_Country_In_Statistics()
        {
            var statistics = _playersRetriever.CalculateStatistics();
            Assert.That(statistics.CountryWithBestWinningRatio!.Code, Is.EqualTo("SRB"));
        }

        [Test]
        public void Application_Should_Get_Right_IMC_Average_In_Statistics()
        {
            
            var statistics = _playersRetriever.CalculateStatistics();
            var expectedAverageIMC = 23.357838995505837;
            Assert.Less(Math.Abs(statistics.AveragePlayersIMC - expectedAverageIMC), NumberCompareThreshold);
        }

        [Test]
        public void Application_Should_Get_Right_Height_Median_Odd_In_Statistics()
        {
            var statistics = _playersRetriever.CalculateStatistics();
            var expectedHeightMedian = 185;
            Assert.Less(Math.Abs(statistics.PlayersHeightMedian - expectedHeightMedian), NumberCompareThreshold);
        }

        [Test]
        public void Application_Should_Get_Right_Height_Median_Even_In_Statistics()
        {
            var playersRetriever = InitializeDependancyInjectionAndGetPlayersRetriever(new PlayersRoot
            {
                Players = new List<Player>
                {
                    new Player { PlayerData = new PlayerData { Height = 2 } },
                    new Player { PlayerData = new PlayerData { Height = 1 } },
                    new Player { PlayerData = new PlayerData { Height = 4 } },
                    new Player { PlayerData = new PlayerData { Height = 3 } }
                }
            });
            var statistics = playersRetriever.CalculateStatistics();
            var expectedHeightMedian = 2.5;
            Assert.Less(Math.Abs(statistics.PlayersHeightMedian - expectedHeightMedian), NumberCompareThreshold);
        }

        private const string HardCodedPlayersJsonForUnitTests = @"
{
  ""players"": [
    {
      ""id"": 52,
      ""firstname"": ""Novak"",
      ""lastname"": ""Djokovic"",
      ""shortname"": ""N.DJO"",
      ""sex"": ""M"",
      ""country"": {
        ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Serbie.png"",
        ""code"": ""SRB""
      },
      ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Djokovic.png"",
      ""data"": {
        ""rank"": 2,
        ""points"": 2542,
        ""weight"": 80000,
        ""height"": 188,
        ""age"": 31,
        ""last"": [1, 1, 1, 1, 1]
      }
    },
    {
      ""id"": 95,
      ""firstname"": ""Venus"",
      ""lastname"": ""Williams"",
      ""shortname"": ""V.WIL"",
      ""sex"": ""F"",
      ""country"": {
        ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/USA.png"",
        ""code"": ""USA""
      },
      ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Venus.webp"",
      ""data"": {
        ""rank"": 52,
        ""points"": 1105,
        ""weight"": 74000,
        ""height"": 185,
        ""age"": 38,
        ""last"": [0, 1, 0, 0, 1]
      }
    },
    {
      ""id"": 65,
      ""firstname"": ""Stan"",
      ""lastname"": ""Wawrinka"",
      ""shortname"": ""S.WAW"",
      ""sex"": ""M"",
      ""country"": {
        ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Suisse.png"",
        ""code"": ""SUI""
      },
      ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Wawrinka.png"",
      ""data"": {
        ""rank"": 21,
        ""points"": 1784,
        ""weight"": 81000,
        ""height"": 183,
        ""age"": 33,
        ""last"": [1, 1, 1, 0, 1]
      }
    },
    {
      ""id"": 102,
      ""firstname"": ""Serena"",
      ""lastname"": ""Williams"",
      ""shortname"": ""S.WIL"",
      ""sex"": ""F"",
      ""country"": {
        ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/USA.png"",
        ""code"": ""USA""
      },
      ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Serena.png"",
      ""data"": {
        ""rank"": 10,
        ""points"": 3521,
        ""weight"": 72000,
        ""height"": 175,
        ""age"": 37,
        ""last"": [0, 1, 1, 1, 0]
      }
    },
    {
      ""id"": 17,
      ""firstname"": ""Rafael"",
      ""lastname"": ""Nadal"",
      ""shortname"": ""R.NAD"",
      ""sex"": ""M"",
      ""country"": {
        ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Espagne.png"",
        ""code"": ""ESP""
      },
      ""picture"": ""https://data.latelier.co/training/tennis_stats/resources/Nadal.png"",
      ""data"": {
        ""rank"": 1,
        ""points"": 1982,
        ""weight"": 85000,
        ""height"": 185,
        ""age"": 33,
        ""last"": [1, 0, 0, 0, 1]
      }
    }
  ]
}
        ";
    }
}