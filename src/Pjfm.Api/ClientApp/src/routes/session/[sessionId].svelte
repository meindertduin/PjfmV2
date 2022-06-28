<script lang="ts">
    import {isOnDetailPage} from "../../store/store.js";
    import {onDestroy, onMount} from "svelte";
    import {connectToGroup, isConnected} from "../../store/playbackSocketClientStore";
    import {page} from '$app/stores';
    isOnDetailPage.update(() => true);
    
    let connected = false;
    const unsubscribe = isConnected.subscribe((value) => {
        if (value) {
            connected = value;
            connectToGroup($page.params.groupId);
        }
    })
    
    onMount(() => {
    })
    
    onDestroy(unsubscribe);
</script>
