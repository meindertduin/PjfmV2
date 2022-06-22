import {UserClient} from "./apiClient";

export let userClient: UserClient;

export function loadClients(): void {
    userClient = new UserClient();
}