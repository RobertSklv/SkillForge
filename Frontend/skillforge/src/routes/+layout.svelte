<script lang="ts">
	import { onMount, type Snippet } from "svelte";
	import Footer from "$components/footer/Footer.svelte";
	import Header from "$components/header/Header.svelte";
	import { loadCurrentUser } from "$lib/stores/currentUserStore";
    import 'bootswatch/dist/vapor/bootstrap.min.css';
    import 'bootstrap-icons/font/bootstrap-icons.min.css';
	import CookieConsentBanner from "$components/cookie-consent/CookieConsentBanner.svelte";
    import '$styles/global.scss';
    import { onNavigate } from '$app/navigation';
	import ToastContainer from "$components/toast-container/ToastContainer.svelte";

    onNavigate((navigation) => {
        if (!document.startViewTransition) return;

        return new Promise((resolve) => {
            document.startViewTransition(async () => {
                resolve();
                await navigation.complete;
            });
        });
    });

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

<ToastContainer />

<style>
    @keyframes fade-in {
        from {
            opacity: 0;
        }
    }

    @keyframes fade-out {
        to {
            opacity: 0;
        }
    }

    @keyframes slide-from-right {
        from {
            transform: translateY(-30px);
        }
    }

    @keyframes slide-to-left {
        to {
            transform: translateY(30px);
        }
    }

    :root::view-transition-old(root) {
        animation:
            90ms cubic-bezier(0.4, 0, 1, 1) both fade-out,
            300ms cubic-bezier(0.4, 0, 0.2, 1) both slide-to-left;
    }

    :root::view-transition-new(root) {
        animation:
            210ms cubic-bezier(0, 0, 0.2, 1) 90ms both fade-in,
            300ms cubic-bezier(0.4, 0, 0.2, 1) both slide-from-right;
    }
</style>