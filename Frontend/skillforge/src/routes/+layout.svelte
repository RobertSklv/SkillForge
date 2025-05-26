<script lang="ts">
	import { onMount, type Snippet } from "svelte";
	import Footer from "../components/footer/Footer.svelte";
	import Header from "../components/header/Header.svelte";
	import { loadCurrentUser } from "$lib/stores/currentUserStore";

    const {
        children
    }: {
        children: Snippet
    } = $props();

    onMount(() => {
        loadCurrentUser();
    });
</script>

<Header />

<main id="main-content" class="container pb-5">
    {@render children()}
</main>

<Footer />

<style>
    #main-content {
        padding-top: calc(3rem + 56px);
        min-height: calc(100vh - 56px);
    }
</style>