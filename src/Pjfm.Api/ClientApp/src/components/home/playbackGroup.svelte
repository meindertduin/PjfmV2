
<script lang="ts">
    import type { PlaybackGroupDto } from '../../services/apiClient';
    import {goto} from "$app/navigation";
    
    export let playbackGroup: PlaybackGroupDto;

    function onPlaybackGroupClick(): void {
        goto(`/session/` + playbackGroup.groupId);
    }
</script>


<div class="container" on:click={onPlaybackGroupClick} >
    <div class="title-container">
        <h3 class="title">Test</h3>
        <span class="listeners-text">{playbackGroup.listenersCount} {playbackGroup.listenersCount === 1 ? 'listener' : 'listeners'}</span>
    </div>
    <div class="content">
        {#if playbackGroup.currentlyPlayingTrack != null}
            <div class="image-container">
                <img src="{playbackGroup.currentlyPlayingTrack.spotifyAlbum.albumImage.url}" alt="album cover" />
            </div>
        {/if}
        {#if playbackGroup.currentlyPlayingTrack != null}
            <div class="group-info-container">
                <div class="currently-playing-title">Now playing:</div>
                <a class="track-title">[ {playbackGroup.currentlyPlayingTrack.title} ]</a>
                <a class="artist-title">[ {playbackGroup.currentlyPlayingTrack.artists.join(', ')} ]</a>
            </div>
        {:else}
            <div class="currently-offline-title">Playback group is currently offline</div>
        {/if}
    </div>
</div>


<style lang="scss">
  @import "../../scss/variables";

  .container {
    padding: 10px;
    background: $color-secondary;
    border-radius: 20px;

    &:hover {
      background: darken($color-secondary, 2);
      cursor: pointer;
    }

    .title-container {
      display: flex;
      flex-direction: row;
      align-items: center;
      justify-content: space-between;

      .title {
        text-transform: uppercase;
        margin: 0 5px 0 10px;
        font-weight: lighter;
        color: $color-light-blue;
      }

      .listeners-text {
        color: $color-purple;
      }
    }

    .content {
      display: flex;
      flex-direction: row;
      align-items: center;
      margin-left: 10px;

      .image-container {
        margin: 10px 10px 10px 0;
        img {
          width: 60px;
          height: 60px;
        }
      }

      .group-info-container {
        display: flex;
        flex-direction: column;

        .currently-playing-title {
          margin-bottom: 10px;
          text-decoration: underline;
        }

        .track-title {
          color: $color-yellow;
          margin-bottom: 4px;
        }

        .artist-title {
          font-size: 12px;
          color: $color-red;
        }
      }

      .currently-offline-title {
        margin: 10px 0;
      }
    }
  }
</style>