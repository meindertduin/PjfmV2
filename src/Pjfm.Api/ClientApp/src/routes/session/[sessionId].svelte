<script lang="ts">
    import {isOnDetailPage} from "../../store/store.js";
    import {onDestroy} from "svelte";
    import { page } from '$app/stores';
    import {isConnected, playbackSocketClient} from "../../store/playbackSocketClientStore";
    isOnDetailPage.update(() => true);
    
    let connected = false;
</script>
