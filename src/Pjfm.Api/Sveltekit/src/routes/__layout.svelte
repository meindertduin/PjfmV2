<script lang="ts">
    import {goto} from "$app/navigation";

    import {UserRole} from "../services/apiClient";
    import {onMount} from "svelte";
    import {loadUser, user} from "../store/userStore";
    import {isOnDetailPage} from "../store/store.js";
    
    let showAuthenticateSpotifyButton = false;
    
    onMount(() => {
        loadUser().then(result => {
            if ($user != null) {
                showAuthenticateSpotifyButton = !$user.roles.includes(UserRole.SpotifyAuth);
            }
        })
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
