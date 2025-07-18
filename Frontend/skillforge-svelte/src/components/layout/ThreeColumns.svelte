<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		mod?: string;
		leftColumn?: Snippet;
		children: Snippet;
		rightColumn?: Snippet;
		hideLeftColumnOnMobile?: boolean;
		hideRightColumnOnMobile?: boolean;
	}

	const {
		mod,
		leftColumn,
		children,
		rightColumn,
		hideLeftColumnOnMobile,
		hideRightColumnOnMobile
	}: Props = $props();
</script>

<div class="row flex-column flex-xl-row {mod}">
	<div
		class="left-column"
		class:d-none={hideLeftColumnOnMobile}
		class:d-xl-block={hideLeftColumnOnMobile}
	>
		{@render leftColumn?.()}
	</div>
	<div class="col-12 middle-column">
		{@render children()}
	</div>
	<div
		class="right-column"
		class:d-none={hideRightColumnOnMobile}
		class:d-xl-block={hideRightColumnOnMobile}
	>
		{@render rightColumn?.()}
	</div>
</div>

<style>
	@media (min-width: 1200px) {
		.left-column {
			flex-basis: 20%;
		}

		.middle-column {
			flex-basis: 52%;
		}

		.right-column {
			flex-basis: 28%;
		}
	}
</style>
