<script lang="ts">
	import Button from '$components/button/Button.svelte';
	import { onMount } from 'svelte';
	import { fade } from 'svelte/transition';

	let show = $state<boolean>(false);

    function createCookie(consent: boolean) {
        document.cookie = `cookie_consent=${consent}; path=/; max-age=` + 60 * 60 * 24 * 365;
    }

	function close() {
		show = false;

        createCookie(false);
	}

	function accept() {
		show = false;

        createCookie(true);
	}

    onMount(() => {
        
        if (!document.cookie.includes('cookie_consent')) {
            setTimeout(() => {
                show = true;
            }, 400);
        }
    });
</script>

<aside
	class="cookie-consent-banner offcanvas offcanvas-bottom"
	class:show
    class:hiding={!show}
	tabindex="-1"
	id="offcanvasBottom"
	aria-labelledby="offcanvasBottomLabel"
	aria-modal={show ? 'true' : undefined}
	role={show ? 'dialog' : undefined}
>
	<div class="offcanvas-header">
		<h2 class="offcanvas-title" id="offcanvasBottomLabel">
            <i class="bi bi-cookie me-2"></i>
            Cookies
        </h2>
		<button type="button" class="btn-close" aria-label="Close" onclick={close}></button>
	</div>
	<div class="offcanvas-body">
		<p>We value your privacy. We use cookies to improve your experience. Do you agree to our cookie policy?</p>
	</div>
    <div class="p-3 text-end">
		<Button color="primary" size="lg" mod="me-3" onclick={accept}>Accept</Button>
		<Button color="secondary" size="lg" onclick={close}>Deny</Button>
    </div>
</aside>
{#if show}
    <div class="offcanvas-backdrop show" transition:fade={{ duration: 150 }}></div>
{/if}