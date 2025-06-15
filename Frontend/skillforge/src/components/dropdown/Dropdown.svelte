<script lang="ts">
	import { clickOutside } from '$lib/util';
	import { type Snippet } from 'svelte';

	interface Props {
		isNav?: boolean;
		buttonSnippet: Snippet;
		children: Snippet;
	}

	const {
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
			class="dropdown-toggle nav-link"
			{onclick}
			type="button"
			aria-label="Button"
			aria-expanded={isOpen}
			use:clickOutside={closeDropdown}
		>
			{@render buttonSnippet()}
		</button>
		<ul class="dropdown-menu" class:show={isOpen}>
			{@render children()}
		</ul>
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
		<ul class="dropdown-menu" class:show={isOpen}>
			{@render children()}
		</ul>
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
	}
</style>
