<script lang="ts">
    import {goto} from "$app/navigation";

    import {UserRole} from "../services/apiClient";
    import {onMount} from "svelte";
    import {loadUser, user} from "../store/userStore";
    import {isOnDetailPage} from "../store/store.js";
    
    let showAuthenticateSpotifyButton = false;
    
    onMount(async () => {
        const user = await loadUser();
        if (user != null) {
            showAuthenticateSpotifyButton = !user.roles.includes(UserRole.SpotifyAuth);
        }
    })
    
    function onBackClick(): void {
        goto('../');
    }
</script>
<nav class="toolbar">
    {#if $isOnDetailPage}
        <button class="back-button" on:click={onBackClick}>{ '<--' }</button>
    {/if}
    {#if $user === null}
        <div class="toolbar-right-info">
            <a href="/user/login" class="flat-button flat-button__round flat-button__green ripple m-xs-x">Login</a>
            <a href="/user/register" class="flat-button flat-button__round flat-button__blue ripple m-xs-x">Register</a>
        </div>
    {:else}
        <div class="toolbar-right-info">
            <div class="user-info">
                <span class="m-m-r user-name">{$user.userName}</span>
            </div>
            <div class="toolbar-links">
                {#if showAuthenticateSpotifyButton}
                    <a class="toolbar-link" href="/api/spotify/authenticate">
                        [ Authenticate Spotify ]
                    </a>
                    <a href="/authentication/logout" class="m-xs-l toolbar-link toolbar-link__red">
                        [ logout ]
                    </a>
                {/if}
            </div>
        </div>
    {/if}
</nav>
<div class="view-container">
    <slot></slot>
</div>

<style lang="scss">
  :global {
    @import "../../scss/styles";
  }

  @import "scss/variables";

  $toolbar-height: 70px;

  .toolbar {
    height: $toolbar-height;
    background: $color-primary;
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    position: fixed;
    width: 100%;
    z-index: 100;
    top: 0;

    .back-button {
      position: absolute;
      left: 20px;
      border-radius: 50%;
      border: 0;
      width: 50px;
      height: 50px;
      display: flex;
      justify-content: center;
      align-items: center;
      font-weight: bold;
      color: $white;
      background: inherit;
      cursor: pointer;
      font-size: 20px;

      &:hover, &:focus {
        background: darken($color-primary, 4);
      }
    }

    .title {
      color: $white;
      font-size: 30px;
    }
  }
  .view-container {
    margin: calc(#{$toolbar-height} + 20px) 5px 10px 5px;
  }

  .toolbar-right-info {
    right: 20px;
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;

    .user-info {
      display: flex;
      color: $color-yellow;
      align-items: center;

      .user-name {
        font-size: 20px;
        text-transform: capitalize;
      }
    }

    .toolbar-links {
      display: flex;
      flex-direction: column;
      justify-content: flex-end;
      align-items: flex-end;

      @media (min-width: $breakpoint-s) {
        flex-direction: row;
      }
    }

    .toolbar-link {
      color: $color-yellow;
      cursor: pointer;
      font-size: 15px;
      text-decoration: none;
      margin-top: 5px;

      &:not(:last-child) {
        margin-right: 5px;
      }

      &__red {
        color: $color-red;
      }
    }
  }
</style>
