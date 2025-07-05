<script lang="ts">
	import type { Snippet } from "svelte";
	import { fade } from "svelte/transition";

    interface Props {
        mod?: string,
        children?: Snippet,
        header?: Snippet,
        footer?: Snippet,
    }

    let {
        mod,
        children,
        header,
        footer,
    }: Props = $props();

    const BORDER_RADIUS: 1 | 2 | 3 | 4 | 5 = 3;
</script>

<section class="card border-dark border-1 rounded-{BORDER_RADIUS} bg-dark {mod}" transition:fade={{ duration: 120 }}>
    {#if header}
        <div class="card-header border-bottom-0 rounded-top-{BORDER_RADIUS}">
            {@render header?.()}
        </div>
    {/if}

    {@render children?.()}

    {#if footer}
        <div class="card-footer border-top-0 rounded-bottom-{BORDER_RADIUS}">
            {@render footer?.()}
        </div>
    {/if}
</section>