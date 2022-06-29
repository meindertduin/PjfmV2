<script lang="ts">
    import {isOnDetailPage} from "../../store/store.js";
    import {onDestroy, onMount} from "svelte";
    import {page} from '$app/stores';
    import {connectToGroup, isConnected, playbackData, playbackIsActive} from "../../store/playbackStore";
    import {playbackClient} from "../../services/clients";
    isOnDetailPage.update(() => true);

    import QueuedTrack from "../../components/session/queuedTrack.svelte";
    import TrackProgressionBar from "../../components/session/trackProgressionBar.svelte";
    import StartListenDialog from "../../components/session/StartListenDialog.svelte";
    
    let connected = false;
    let sessionPageDialogOpen = false;
    let showStartListenDialog = false;
    let isRequestingSkip = false;
    
    const unsubscribe = isConnected.subscribe((value) => {
        if (value) {
            connected = value;
            connectToGroup($page.params.sessionId);
        }
    })
    
    let trackStartTimeMs = 0;
    
    $: {
        if ($playbackData != null) {
            const trackStartDate = new Date($playbackData.currentlyPlayingTrack.trackStartDate);
            trackStartTimeMs = new Date().getTime() - trackStartDate.getTime();
        }
    }
    
    onMount(() => {
    })
    
    function play(): void {
        if ($playbackData?.groupId == null || sessionPageDialogOpen) {
            return;
        }

        
        sessionPageDialogOpen = true;
        showStartListenDialog = true;
    }
    
    function closePlayDialog(): void {
        sessionPageDialogOpen = false;
        showStartListenDialog = false;
    }
    
    function pause(): void {
        playbackClient.stop().then(() => {
            playbackIsActive.update(() => false);
        });
    }
    
    function skip(): void {
        if (isRequestingSkip) return;
        
        isRequestingSkip = true;
        playbackClient.skip().finally(() => {
            isRequestingSkip = false;
        });
    }
    
    function settings() {
        if (sessionPageDialogOpen) return;
        
        sessionPageDialogOpen = true;
        // Do request for opening dialog
    }
    
    
    onDestroy(unsubscribe);
</script>

<div class="container">
    <section class="playback">
        {#if $playbackData != null}
            <div class="playback-info">
                <div class="image-container">
                    <img src="{$playbackData.currentlyPlayingTrack.spotifyAlbum.albumImage.url}" alt="album cover" />
                </div>
                <div class="track-info-container">
                    <div class="currently-playing-title">Now playing:</div>
                    <a class="track-title">[ <span class="ellipsis-title">{$playbackData.currentlyPlayingTrack.title}</span> ]</a>
                    <a class="artist-title">[ <span class="ellipsis-title">{$playbackData.currentlyPlayingTrack.artists.join(', ')}</span> ]</a>
                </div>
            </div>
        {/if}
        {#if $playbackData != null}
            <div class="playback-controls-container">
               <TrackProgressionBar startTimeMs="{trackStartTimeMs}" trackDurationMs="{$playbackData.currentlyPlayingTrack.trackDurationMs}"></TrackProgressionBar>
                <div class="playback-buttons">
                    {#if $playbackIsActive}
                        <button class="flat-button ripple control-button" on:click="{pause}">
                            Pause
                        </button>
                    {:else}
                        <button class="flat-button ripple control-button" on:click="{play}">
                            play
                        </button>
                    {/if}
                </div>
            </div>
        {/if}
    </section>
    <section class="playback-queue">
        <h4>Up next:</h4>
        {#if $playbackData != null}
            <div class="queue-container" >
                {#each $playbackData.queuedTracks as queuedTrack}
                    <div class="queued-track">
                        <QueuedTrack track="{queuedTrack}"></QueuedTrack>
                    </div>
                {/each}
            </div>
        {/if}
    </section>
</div>

{#if showStartListenDialog}
    <StartListenDialog on:closeDialog={closePlayDialog}></StartListenDialog>
{/if}

<style lang="scss">
  @import "scss/variables";

  .container {
    margin: 20px;

    .playback {
      margin-bottom: 20px;
      flex-direction: row;
      display: flex;
      flex-wrap: wrap;

      .playback-info {
        display: flex;

        .image-container {
          margin-right: 20px;
          img {
            width: 80px;
            height: 80px;
          }
        }

        .track-info-container {
          display: flex;
          flex-direction: column;
          width: 180px;

          @media (min-width: $breakpoint-xs) {
            width: 200px;
          }

          @media (min-width: $breakpoint-m) {
            width: 250px;
          }

          .currently-playing-title {
            font-size: 20px;
            margin-bottom: 10px;
            text-decoration: underline;
          }

          .track-title {
            margin-bottom: 5px;
            color: $color-yellow;
            display: flex;
          }

          .artist-title {
            font-size: 14px;
            color: $color-red;
            display: flex;
          }
        }
      }

      .playback-controls-container {
        margin-top: 20px;
        display: flex;
        flex-direction: column;
        width: 100%;

        @media screen and (min-width: $breakpoint-m) {
          margin-left: 40px;
          width: 60%;
          margin-top: 0;
        }
      }
    }

    .playback-queue {
      h4 {
        margin: 0 0 10px 0;
      }

      .queue-container {
        display: flex;
        flex-direction: column;
        
        .queued-track {
          margin-bottom: 10px
        }
      }
    }
  }

  .playback-buttons {
    display: flex;
    flex-direction: row;
    width: 100%;
    justify-content: center;

    @media screen and (min-width: $breakpoint-m) {
      justify-content: flex-start;
    }

    .control-button {
      margin: 0 10px;

      &:nth-child(1) {
        margin: 0 10px 0 0;
      }

      &:last-child {
        margin: 0 0 0 10px;
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