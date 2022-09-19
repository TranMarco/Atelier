using Application.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [CustomExceptionFilter]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AtelierController: ControllerBase
    {
        private readonly IPlayersRetriever _playersRetriever;

        public AtelierController(IPlayersRetriever playersRetriever)
        {
            _playersRetriever = playersRetriever;
        }

        [HttpGet(Name = "GetException")]
        public void GetException()
        {
            throw new ArgumentException("Exception");
        }

        [HttpGet(Name = "GetSortedPlayersFromBestToWorst")]
        public List<Player> GetSortedPlayersFromBestToWorst()
        {
            return _playersRetriever.GetSortedPlayersFromBestToWorst();
        }

        [HttpGet(Name = "GetPlayerFromId")]
        public Player GetPlayerFromId(int id)
        {
            return _playersRetriever.GetPlayerFromId(id);
        }

        [HttpGet(Name = "CalculateStatistics")]
        public Statistics CalculateStatistics()
        {
            return _playersRetriever.CalculateStatistics();
        }
    }
}
