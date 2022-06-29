<script lang="ts">
   import AsciiSlider from "./AsciiSlider.svelte";

   export let trackDurationMs!: number;
   export let startTimeMs = 0;
   
   let currentInterval!: any;
   let currentTimeMs!: number;
   let trackPercentage = 0;
   
   function formatTimeMs(value: number): string {
       const seconds = Math.floor(value / 1000);
       const minutes = Math.floor(seconds / 60);
       const leftOverSeconds = seconds - minutes * 60;

       return `${Math.round(minutes)}:${leftOverSeconds < 10 ? '0' : ''}${Math.round(leftOverSeconds)}`;
   }
   
   $: {
       if (trackDurationMs)
           reset();
   }
   
   function reset() {
       currentTimeMs = startTimeMs;
       
       if (currentInterval != null) {
            clearInterval(currentInterval);    
       }

       setTimeOut();
   }
   
   function setTimeOut() {
       currentInterval = setInterval(() => {
           if (currentTimeMs < trackDurationMs) {
               currentTimeMs += 1000;
               trackPercentage = Math.round((currentTimeMs  / trackDurationMs) * 100);
           }
       }, 1000);
   }
</script>

<div class="container">
    
    <AsciiSlider percentage="{trackPercentage}"></AsciiSlider>
    
    <div class="time-labels-container">
        <div class="time-label">{formatTimeMs(currentTimeMs)}</div>
        <div class="time-label">{ formatTimeMs(trackDurationMs)}</div>
    </div>
</div>
