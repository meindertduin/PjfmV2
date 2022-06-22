<script lang="ts">
    import PlaybackGroup from "../components/home/playbackGroup.svelte";
    import {onMount} from "svelte";
    import {PlaybackClient} from "../services/apiClient";
    import type { PlaybackGroupDto } from '../services/apiClient';
    import {isOnDetailPage} from "../store/store";

    isOnDetailPage.update(() => false);

    let playbackGroups: PlaybackGroupDto[] = [];
    
    onMount(() => {
        let playbackClient = new PlaybackClient();
        playbackClient.groups().then((result) => {
            playbackGroups = result;
        });
    });
</script>

<div class="container">
    <div class="playgroup-container">
        {#each playbackGroups as playbackGroup}
            <PlaybackGroup playbackGroup={playbackGroup}></PlaybackGroup>
        {/each}
    </div>
</div>

<style lang="scss">
</style>
