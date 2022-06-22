<script>
	import Home from "./views/Home.svelte";
	import { Router, Route } from "svelte-routing";
	import DefaultLayout from "./layouts/DefaultLayout.svelte";

	export let url = "";
</script>

<Router url="{url}">
	<DefaultLayout>
		<Route path="/" component="{Home}" />
	</DefaultLayout>
</Router>

