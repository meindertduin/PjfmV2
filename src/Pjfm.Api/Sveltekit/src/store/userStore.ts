import {UserClient} from "../services/apiClient";
import type {GetCurrentUserResponseModel} from "../services/apiClient";
import {writable} from "svelte/store";
import type {Writable} from "svelte/store";

export let user: Writable<GetCurrentUserResponseModel | null> = writable(null);

export async function loadUser(): Promise<GetCurrentUserResponseModel | null> {
    let userClient = new UserClient();
    try {
        const loadedUser = await userClient.me();
        user.update(() => loadedUser);
        return loadedUser;
    } catch (error: unknown) {
        user.update(() => null);
        return null;
    }
}