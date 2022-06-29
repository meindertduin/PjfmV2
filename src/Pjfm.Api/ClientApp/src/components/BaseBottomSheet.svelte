<script lang="ts">
    import { createEventDispatcher } from 'svelte';

    const dispatch = createEventDispatcher();

    function closeDialog(): void {
        dispatch('closeDialog');
    }
</script>

<div class="container">
    <div class="dialog">
        <div class="dialog-header-container">
            <slot name="title"></slot>
            <button class="icon-button" on:click="{closeDialog}">[ X ]</button>
        </div>
        <div class="dialog-content-container">
            <slot name="content"></slot>
        </div>
        <div class="dialog-footer-container">
            <slot name="footer"></slot>
        </div>
    </div>
</div>
<div class="overlay"></div>

<style lang="scss">
    
@import "scss/variables";

$dialog-header-height: 40px;
$dialog-footer-height: 50px;

.container {
    position: fixed;
    bottom: 0;
    left: 50%;
    transform: translateX(-50%);
    width: 100%;
    background: $color-primary;
    color: $white;
    max-height: 90vh;
    border-radius: 30px 30px 0 0;
    z-index: 1010;

    @media (min-width: $breakpoint-m) {
    width: 800px;
    bottom: 40%;
    border-radius: 20px;
}

    .dialog {
    padding: 5px 10px 5px 10px;
}

    .dialog-header-container {
    display: flex;
    flex-direction: row;
    height: $dialog-header-height;
    justify-content: space-between;
    align-items: center;
}

    .dialog-content-container {
    max-height: calc(85vh - #{$dialog-header-height + $dialog-footer-height});
    overflow-y: auto;
}

    .dialog-footer-container {
    height: $dialog-footer-height;
}
}

.overlay {
    width: 100%;
    height: 100%;
    position: fixed;
    z-index: 1000;
    background: black;
    opacity: .6;
    top: 0;

    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
}
</style>
