import {writable} from "svelte/store";
import type {Writable} from "svelte/store";

export let isOnDetailPage: Writable<boolean> = writable(false);
