using Microsoft.Extensions.DependencyInjection;
using Moq;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Services;
using Xunit;

namespace Pjfm.SpotifyPlayback.Test.Services
{
    public class PlaybackGroupCollectionTest
    {
        private readonly Mock<IPlaybackQueue> _mockPlaybackQueue;

        public PlaybackGroupCollectionTest()
        {
            _mockPlaybackQueue = new Mock<IPlaybackQueue>();
        }

        [Fact]
        public void CreateNewPlaybackGroup_Should_AddNewPlaybackGroup()
        {
            // Arrange
            var groupName1 = "test1";
            var groupName2 = "test2";
            var playbackGroupCollection = CreatePlaybackGroupCollection();
            playbackGroupCollection.PlaybackGroupCreatedEvent += (sender, args) => { };

            // Act
            playbackGroupCollection.CreateNewPlaybackGroup(groupName1);
            playbackGroupCollection.CreateNewPlaybackGroup(groupName2);

            var playbackGroups = playbackGroupCollection.GetPlaybackGroupsInfo();
            
            // Assert
            Assert.Contains(playbackGroups, x => x.GroupName == groupName1);
            Assert.Contains(playbackGroups, x => x.GroupName == groupName2);
        }
        
        private PlaybackGroupCollection CreatePlaybackGroupCollection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(x => _mockPlaybackQueue.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            return new PlaybackGroupCollection(serviceProvider);
        }
    }
}