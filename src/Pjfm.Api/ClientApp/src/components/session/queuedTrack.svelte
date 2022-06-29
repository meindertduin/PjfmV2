<script lang="ts">
    import type {SpotifyTrackDto} from "../../services/apiClient";
    export let track!: SpotifyTrackDto;
</script>

<div class="container">
    <div class="image-container">
        <img src="{track.spotifyAlbum.albumImage.url}" alt="album cover" />
    </div>
    <div class="track-info-container">
        <a class="track-title">[ <span class="ellipsis-title">{track.title}</span> ]</a>
        <a class="artist-title">[ <span class="ellipsis-title">{track.artists.join(', ')}</span> ]</a>
    </div>
</div>

<style lang="scss">
  @import "scss/variables";

  .container {
    background: $color-secondary;
    border-radius: 0 20px 0 20px;
    padding: 10px 20px;
    display: flex;
    flex-direction: row;

    &:hover {
      background: darken($color-secondary, 2);
    }

    .image-container {
      margin-right: 10px;
      img {
        width: 40px;
        height: 40px;
      }
    }

    .track-info-container {
      display: flex;
      flex-direction: column;
      margin-top: 5px;
      width: 80%;

      .track-title {
        margin-bottom: 5px;
        color: $color-light-blue;
        display: flex;
      }

      .artist-title {
        font-size: 14px;
        color: $color-orange;
        display: flex;
      }
    }
  }

  .ellipsis-title {
    max-width: 90%;
    overflow-x: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
  }
</style>