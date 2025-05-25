<script lang="ts">
	import type SiteData from "$lib/types/SiteData";
	import { setContext } from "svelte";

    const {
        data
    }: {
        data: SiteData
    } = $props();

    setContext('currentUser', data.currentUser);
</script>

<h1>Welcome to SvelteKit</h1>
<p>Visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to read the documentation</p>

{#if data.currentUser}
    <span>Logged in</span>
{:else}
    <span>Not logged in</span>
{/if}