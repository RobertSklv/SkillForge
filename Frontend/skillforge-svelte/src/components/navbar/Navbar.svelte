<script lang="ts">
	import { onNavigate } from '$app/navigation';
	import { clickOutside } from '$lib/svelte-util';
	import type { Snippet } from 'svelte';
	import { fade, fly } from 'svelte/transition';

	interface Props {
		logoLink: string;
		logoSnippet: Snippet;
		children?: Snippet;
		linksSnippet?: Snippet;
	}

	const { logoLink, logoSnippet, children, linksSnippet }: Props = $props();

	let isOpen = $state(false);
	let windowWidth = $state<number>(1400);
	let isHamburgerMenu = $derived<boolean>(windowWidth <= 991);

	function onTogglerClick() {
		isOpen = !isOpen;
	}

	function closeMenu() {
		isOpen = false;
	}
	
    onNavigate(() => {
        closeMenu();
    });
</script>

<svelte:window bind:innerWidth={windowWidth} />

<nav class="navbar navbar-expand-lg fixed-top bg-primary navbar-main" use:clickOutside={closeMenu}>
	<div class="container">
		<a class="navbar-brand" href={logoLink} title="To homepage">
			{@render logoSnippet()}
		</a>
		<div class="d-flex justify-content-center flex-shrink-1" style:flex-grow="0.5">
			<div class="w-100">
				{@render children?.()}
			</div>
		</div>
		<button
			class="navbar-toggler"
			class:collapsed={!isOpen}
			onclick={onTogglerClick}
			type="button"
			aria-controls="navbarNavDropdown"
			aria-expanded="false"
			aria-label="Toggle navigation"
		>
			<span class="navbar-toggler-icon"></span>
		</button>

		{#if !isHamburgerMenu || isOpen}
			<div
				class="collapse navbar-collapse justify-content-end flex-shrink-0 flex-grow-0 {isHamburgerMenu
					? 'position-absolute start-0 end-0 navbar bg-primary'
					: ''}"
				class:show={isOpen}
				style:top={isHamburgerMenu ? '54px' : 'auto'}
				id="navbarNavDropdown"
				in:fly={{ y: -15, duration: 100 }}
				out:fly={{ y: -15, duration: 100 }}
			>
				<ul
					class="navbar-nav {isHamburgerMenu
						? 'container text-start justify-content-start align-items-start'
						: ''}"
					style:padding-left={isHamburgerMenu ? '0.75rem' : 'inherit'}
				>
					{@render linksSnippet?.()}
				</ul>
			</div>
		{/if}
	</div>
</nav>

<style>
	.navbar-main {
		view-transition-name: header;
	}
</style>
