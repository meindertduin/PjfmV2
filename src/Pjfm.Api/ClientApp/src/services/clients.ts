import {PlaybackClient, UserClient} from "./apiClient";

export let userClient = new UserClient(undefined, { fetch });
export let playbackClient = new PlaybackClient(undefined, { fetch });
