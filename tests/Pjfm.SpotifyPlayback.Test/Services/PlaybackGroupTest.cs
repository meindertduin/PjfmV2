using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Pjfm.Application.GebruikerNummer.Models;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.Socket;
using SpotifyPlayback.Services;
using Xunit;

namespace Pjfm.SpotifyPlayback.Test.Services
{
    public class PlaybackGroupTest
    {
        private readonly Mock<IPlaybackQueue> _mockPlaybackQueue;

        public PlaybackGroupTest()
        {
            _mockPlaybackQueue = new Mock<IPlaybackQueue>();
        }

        [Fact]
        public async Task GetNextTrack_Should_SetRightTracks()
        {
            // Arrange
            var mockTracks = new Queue<SpotifyTrackDto>();
            for (int i = 0; i < 3; i++)
            {
                mockTracks.Enqueue(new SpotifyTrackDto() { Title = i.ToString() });
            }

            _mockPlaybackQueue.SetupSequence(queue => queue.GetNextSpotifyTrack())
                .Returns(Task.FromResult(mockTracks.Dequeue()))
                .Returns(Task.FromResult(mockTracks.Dequeue()))
                .Returns(Task.FromResult(mockTracks.Dequeue()));
            
            var playbackGroup = CreatePlaybackGroup();
            
            for (int i = 0; i < 3; i++)
            {
                // Act
                var track = await playbackGroup.GetNextTrack();
                
                // Assert
                Assert.Equal(i.ToString(), track.Title);
            }
        }
        private PlaybackGroup CreatePlaybackGroup()
        {
            var playbackGroup = new PlaybackGroup(_mockPlaybackQueue.Object, "0321", "test");
            return playbackGroup;
        }
    }
}