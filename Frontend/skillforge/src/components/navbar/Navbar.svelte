<script lang="ts">
	import type { Snippet } from 'svelte';

	const {
		logoLink,
		logoSnippet,
		children
	}: {
		logoLink: string;
		logoSnippet: Snippet;
		children: Snippet;
	} = $props();

	let isOpen = $state(false);

	function onTogglerClick() {
		isOpen = !isOpen;
	}
</script>

<nav class="navbar navbar-expand-lg fixed-top bg-primary">
	<div class="container">
		<a class="navbar-brand" href={logoLink}>
			{@render logoSnippet()}
		</a>
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
		<div
			class="collapse navbar-collapse justify-content-end"
			class:show={isOpen}
			id="navbarNavDropdown"
		>
			<ul class="navbar-nav">
				{@render children()}
			</ul>
		</div>
	</div>
</nav>

<style>
	.navbar {
		view-transition-name: header;
	}
</style>
