<script lang="ts">
	import type { Snippet } from "svelte";

    interface Props {
        mod?: string,
        leftColumn?: Snippet,
        children: Snippet,
        rightColumn?: Snippet,
    }

    const {
        mod,
        leftColumn,
        children,
        rightColumn,
    }: Props = $props();
</script>

<div class="row {mod}">
    <div class="d-none d-lg-block left-column">
        {@render leftColumn?.()}
    </div>
    <div class="col-12 middle-column">
        {@render children()}
    </div>
    <div class="d-none d-lg-block right-column">
        {@render rightColumn?.()}
    </div>
</div>

<style>
	@media (min-width: 992px) {
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