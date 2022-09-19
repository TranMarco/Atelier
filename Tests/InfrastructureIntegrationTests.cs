using Application.Interfaces;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    internal class InfrastructureIntegrationTests
    {
        private IPlayersRepository _playersRepository;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IPlayersRepository, PlayersRepository>();

            var serviceProvider = services.BuildServiceProvider();

            _playersRepository = serviceProvider.GetService<IPlayersRepository>() ?? throw new ArgumentNullException();
        }

        [Test]
        public void GetPlayersRoot()
        {
            var playersRoot = _playersRepository.GetPlayersRoot();
            Assert.That(playersRoot.Players.Count, Is.EqualTo(5));
        }
    }
}
