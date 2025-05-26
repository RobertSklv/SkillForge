<script lang="ts">
	import type { Snippet } from "svelte";
	import type { MouseEventHandler } from "svelte/elements";

    interface Props {
        href?: string,
        isButton?: boolean,
        onclick?: MouseEventHandler<HTMLButtonElement>,
        active?: boolean,
        disabled?: boolean,
        children: Snippet
    }

    const {
        href,
        isButton,
        active,
        disabled,
        onclick,
        children
    }: Props = $props();
</script>

<li>
    {#if isButton}
        <button class="dropdown-item" class:active class:disabled {onclick}>
            {@render children()}
        </button>
    {:else}
        <a class="dropdown-item" class:active class:disabled {href}>
            {@render children()}
        </a>
    {/if}
</li>