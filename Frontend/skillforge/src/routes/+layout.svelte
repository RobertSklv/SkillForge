<script lang="ts">
	import Footer from "../components/footer/Footer.svelte";
	import Header from "../components/header/Header.svelte";

    const {
        children
    } = $props();
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