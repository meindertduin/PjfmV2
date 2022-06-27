import type {GetCurrentUserResponseModel} from "../services/apiClient";
import {writable} from "svelte/store";
import type {Writable} from "svelte/store";
import {userClient} from "../services/clients";

export let user: Writable<GetCurrentUserResponseModel | null> = writable(null);

export async function loadUser(): Promise<GetCurrentUserResponseModel | null> {
    try {
        const loadedUser = await userClient.me();
        user.update(() => loadedUser);
        return loadedUser;
    } catch (error: unknown) {
        user.update(() => null);
        return null;
    }
}