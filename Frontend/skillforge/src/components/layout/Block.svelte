<script lang="ts">
	import type { Snippet } from "svelte";

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

    const BORDER_RADIUS: 1 | 2 | 3 | 4 | 5 = 4;
</script>

<div class="card border-dark rounded-{BORDER_RADIUS} bg-dark {mod}">
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
</div>