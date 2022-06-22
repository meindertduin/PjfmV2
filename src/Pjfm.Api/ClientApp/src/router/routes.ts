import Home from "../views/Home.svelte";

export interface Route {
    component: any;
    path: string;   
}

export const routes: Route[] = [
    {
        component: Home,
        path: '/',
    },
]