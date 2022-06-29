<script lang="ts">
    import {onMount} from "svelte";

    export let percentage = 0;

    let loadingBar = '';
    let componentHasLoaded = false;

    let containerWidth = 0;
    
    $: {
        if (percentage) {
            setLoadingBar();
        }
    }

    function setLoadingBar() {
        if (!componentHasLoaded) {
            return;
        }

        if (percentage > 100) {
            percentage = 100;
        }

        const charactersAmount = Math.floor(containerWidth / 9.7);

        loadingBar = '[';
        let xAmount = (charactersAmount - 2) * (percentage / 100.0);
        for (let i = 1; i < charactersAmount - 1; i++) {
            if (xAmount > 0) {
                loadingBar += 'x';
                xAmount--;
            } else {
                loadingBar += ' ';
            }
        }
        loadingBar += ']';
    }

    onMount(() => {
        componentHasLoaded = true;
        setLoadingBar();
    })

</script>

<svelte:window on:resize={setLoadingBar}></svelte:window>

<div class="container" bind:offsetWidth={containerWidth}>
    <pre>{loadingBar}</pre>
</div>

<style lang="scss">
  @import "scss/variables";

  .container {
    width: 100%;
    display: flex;
    justify-content: center;
    color: $color-purple;

    pre {
      margin-top: 0;
    }
  }
</style>
