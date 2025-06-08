<script lang="ts">
	import { onMount, type Snippet } from "svelte";
	import Footer from "$components/footer/Footer.svelte";
	import Header from "$components/header/Header.svelte";
	import { loadCurrentUser } from "$lib/stores/currentUserStore";
    import 'bootswatch/dist/vapor/bootstrap.min.css';
    import 'bootstrap-icons/font/bootstrap-icons.min.css';
	import CookieConsentBanner from "$components/cookie-consent/CookieConsentBanner.svelte";
    import '$styles/global.scss';

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

<CookieConsentBanner />