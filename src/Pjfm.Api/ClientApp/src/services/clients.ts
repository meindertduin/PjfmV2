import {PlaybackClient, UserClient} from "./apiClient";

export const userClient = new UserClient(undefined, { fetch });
export const playbackClient = new PlaybackClient(undefined, { fetch });