import type {PlaybackUpdateMessageBody} from "../services/playbackSocketClient";
import {readable, writable} from "svelte/store";
import type {Writable} from "svelte/store";
import {PlaybackSocketClient} from "../services/playbackSocketClient";

export let isConnected: Writable<boolean> = writable(false);
export let playbackData: Writable<PlaybackUpdateMessageBody | null> = writable(null);
export let playbackSocketClient = readable(new PlaybackSocketClient())
