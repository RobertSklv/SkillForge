<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		logoLink: string;
		logoSnippet: Snippet;
		children?: Snippet;
		linksSnippet?: Snippet,
	}

	const {
		logoLink,
		logoSnippet,
		children,
		linksSnippet,
	}: Props = $props();

	let isOpen = $state(false);

	function onTogglerClick() {
		isOpen = !isOpen;
	}
</script>

<nav class="navbar navbar-expand-lg fixed-top bg-primary">
	<div class="container">
		<a class="navbar-brand" href={logoLink} title="To homepage">
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

		<div class="d-flex justify-content-center w-100">
			{@render children?.()}
		</div>

		<div
			class="collapse navbar-collapse justify-content-end flex-shrink-0"
			class:show={isOpen}
			id="navbarNavDropdown"
		>
			<ul class="navbar-nav">
				{@render linksSnippet?.()}
			</ul>
		</div>
	</div>
</nav>

<style>
	.navbar {
		view-transition-name: header;
	}
</style>
