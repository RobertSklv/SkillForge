<script lang="ts">
	import { clickOutside } from '$lib/svelte-util';
	import { type Snippet } from 'svelte';
	import { fade } from 'svelte/transition';

	interface Props {
		menuClass?: string,
		isNav?: boolean;
		buttonSnippet: Snippet;
		children: Snippet;
	}

	const {
		menuClass,
        isNav,
        buttonSnippet,
        children
    }: Props = $props();

	let isOpen = $state(false);

	function onclick() {
		isOpen = !isOpen;
	}

	function closeDropdown() {
		isOpen = false;
	}
</script>

{#if isNav}
	<li class="dropdown nav-item">
		<button
			class="dropdown-toggle nav-link text-start w-100"
			{onclick}
			type="button"
			aria-label="Button"
			aria-expanded={isOpen}
			use:clickOutside={closeDropdown}
		>
			{@render buttonSnippet()}
		</button>
		{#if isOpen}
			<ul class="dropdown-menu show {menuClass}" data-bs-popper="static" transition:fade={{ duration: 100 }}>
				{@render children()}
			</ul>
		{/if}
	</li>
{:else}
	<div class="btn-group">
		<button
			class="dropdown-toggle btn btn-outline-primary rounded-3 border-1"
			{onclick}
			type="button"
			aria-label="Button"
			aria-expanded={isOpen}
			use:clickOutside={closeDropdown}
		>
			{@render buttonSnippet()}
		</button>
		{#if isOpen}
			<ul class="dropdown-menu show {menuClass}" data-bs-popper="static" transition:fade={{ duration: 100 }}>
				{@render children()}
			</ul>
		{/if}
	</div>
{/if}

<style lang="scss">
	.dropdown-toggle {
		&::after {
			display: none;
		}
	}

	.dropdown-menu {
		top: 100%;
		left: 0;
		margin-top: var(--bs-dropdown-spacer);
		background-color: #30115e;
	}
</style>
