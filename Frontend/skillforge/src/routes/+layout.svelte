<script lang="ts">
	import { type Snippet } from "svelte";
	import Footer from "$components/footer/Footer.svelte";
	import Header from "$components/header/Header.svelte";
    import { currentUserStore } from "$lib/stores/currentUserStore";
    import 'bootswatch/dist/vapor/bootstrap.min.css';
    import 'bootstrap-icons/font/bootstrap-icons.min.css';
	import CookieConsentBanner from "$components/cookie-consent/CookieConsentBanner.svelte";
    import '$styles/global.scss';
    import { onNavigate } from '$app/navigation';
	import ToastContainer from "$components/toast-container/ToastContainer.svelte";
	import { getFrontendUrl } from "$lib/util";
	import type UserInfo from "$lib/types/UserInfo";

    onNavigate((navigation) => {
        if (!document.startViewTransition) return;

        // Disabled view transitions when only query param changes are present.
        if (navigation.from?.url.pathname === navigation.to?.url.pathname) return;

        return new Promise((resolve) => {
            document.startViewTransition(async () => {
                resolve();
                await navigation.complete;
            });
        });
    });

    interface Props {
        data?: UserInfo,
        children: Snippet
    }

    let {
        data,
        children
    }: Props = $props();
    
    if (data && Object.keys(data).length === 0) {
        data = undefined;
    }
    
    currentUserStore.set(data);
</script>

<Header />

<main id="main-content" class="container pb-5">
    {@render children()}
</main>

<Footer />

<CookieConsentBanner />

<ToastContainer />

{@html `<script type="application/ld+json">
    ${JSON.stringify({
        '@context': 'https://schema.org',
        '@type': 'WebSite',
        name: 'SkillForge',
        description: "Discover and share high-quality articles on technology, development, and more. Follow tags and authors, engage with the community, and explore trending content.",
        url: getFrontendUrl()
    })}
</script>`}

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