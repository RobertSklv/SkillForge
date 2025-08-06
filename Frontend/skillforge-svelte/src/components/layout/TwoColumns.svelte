<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		mod?: string;
		leftColumn?: Snippet;
		children: Snippet;
	}

	const { mod, leftColumn, children }: Props = $props();
</script>

<div class="row {mod}">
	<div class="d-none d-lg-block left-column">
		{@render leftColumn?.()}
	</div>
	<div class="col-12 main-column">
		{@render children()}
	</div>
</div>

<style>
	@media (min-width: 992px) {
		.left-column {
			flex-basis: 25%;
		}

		.main-column {
			flex-basis: 75%;
		}
	}
</style>
