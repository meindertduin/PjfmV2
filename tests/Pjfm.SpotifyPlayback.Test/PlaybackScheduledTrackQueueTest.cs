using System;
using System.Linq;
using Pjfm.Application.GebruikerNummer.Models;
using SpotifyPlayback;
using SpotifyPlayback.Models;
using Xunit;

namespace Pjfm.SpotifyPlayback.Test
{
    public class PlaybackScheduledTrackQueueTest
    {
        private readonly PlaybackScheduledTrackQueue _playbackScheduledTrackQueue;

        public PlaybackScheduledTrackQueueTest()
        {
            _playbackScheduledTrackQueue = new PlaybackScheduledTrackQueue();
        }

        [Fact]
        public void AddPlaybackScheduledTrack_Should_InsertTrackAtRightIndex()
        {
            // Arrange
            var groupId = "0312";
            var tracksToSchedule = GetAddPlaybackScheduledTracksTestCaseTracks(groupId);

            var expectedScheduledTrackTitlesOrder = new[] {"0", "3", "2", "1"};

            // Act
            foreach (var scheduledTrack in tracksToSchedule)
            {
                _playbackScheduledTrackQueue.AddPlaybackScheduledTrack(scheduledTrack);
            }

            var scheduledTracksInOrder = _playbackScheduledTrackQueue.GetScheduledTracks().ToArray();
            
            // Assert
            for (int i = 0; i < 4; i++)
            {
                Assert.Equal(expectedScheduledTrackTitlesOrder[i], scheduledTracksInOrder[i].SpotifyTrack.Title);
            }
        }

        [Fact]
        public void GetDueTracks_Should_ReturnOnlyDueTracks()
        {
            // Arrange
            var groupId = "0321";
            var testTracks = GetGetDueTracksTestTracks(groupId);

            for (int i = 0; i < 4; i++)
            {
                _playbackScheduledTrackQueue.AddPlaybackScheduledTrack(testTracks[i]);
            }

            // Act
            var dueTracks = _playbackScheduledTrackQueue.GetDueTracks();
            
            // Assert
            Assert.Contains(dueTracks, x => x.SpotifyTrack.Title == "0");
            Assert.Contains(dueTracks, x => x.SpotifyTrack.Title == "1");
            Assert.Contains(dueTracks, x => x.SpotifyTrack.Title == "3");
            
            Assert.DoesNotContain(dueTracks, x => x.SpotifyTrack.Title == "2");
        }

        private static PlaybackScheduledTrack[] GetGetDueTracksTestTracks(string groupId)
        {
            var testTracks = new[]
            {
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "0"},
                    GroupId = groupId,
                    DueTime = DateTime.Now - TimeSpan.FromSeconds(2),
                },
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "1"},
                    GroupId = groupId,
                    DueTime = DateTime.Now - TimeSpan.FromSeconds(1),
                },
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "2"},
                    GroupId = groupId,
                    DueTime = DateTime.Now + TimeSpan.FromSeconds(10),
                },
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "3"},
                    GroupId = groupId,
                    DueTime = DateTime.Now - TimeSpan.FromMilliseconds(1),
                },
            };
            return testTracks;
        }

        private static PlaybackScheduledTrack[] GetAddPlaybackScheduledTracksTestCaseTracks(string groupId)
        {
            var tracksToSchedule = new[]
            {
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "0"},
                    GroupId = groupId,
                    DueTime = DateTime.Now,
                },
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "1"},
                    GroupId = groupId,
                    DueTime = DateTime.Now + TimeSpan.FromMinutes(2),
                },
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "2"},
                    GroupId = groupId,
                    DueTime = DateTime.Now + TimeSpan.FromMinutes(1),
                },
                new PlaybackScheduledTrack()
                {
                    SpotifyTrack = new SpotifyTrackDto() {Title = "3"},
                    GroupId = groupId,
                    DueTime = DateTime.Now + TimeSpan.FromSeconds(2),
                },
            };
            return tracksToSchedule;
        }

    }
}