<script lang="ts">
    import BaseBottomSheet from "../BaseBottomSheet.svelte";
    import { createEventDispatcher } from 'svelte';

    const dispatch = createEventDispatcher();

    function closeDialog(): void {
        dispatch('closeDialog');
    }
    
    function play() {
        
    }
     
</script>

<BaseBottomSheet on:closeDialog>
    <h4 slot="title" class="m-0">Set play time</h4>
    <div slot="content" class="container">
        <div class="form-container">
            
        </div>
    </div>
    <div slot="footer" class="button-container">
        <button class="raised-button raised-button__danger" on:click="{closeDialog}"><span>Cancel</span></button>
        <button class="raised-button raised-button__warning" on:click="{play}"><span>Play</span></button>
    </div>
</BaseBottomSheet>

<style lang="scss">
  .container {
    padding: 10px 15px 30px 15px;
    display: flex;
    justify-content: center;


    .form-container {
      width: 80%;
    }
  }

  .button-container {
    padding: 0 15px;
    display: flex;
    justify-content: center;
    flex-direction: row;

    button {
      width: 40%;

      &:first-child {
        margin-right: 10px;
      }
    }
  }
</style>