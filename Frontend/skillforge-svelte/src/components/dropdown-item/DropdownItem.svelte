<script lang="ts">
	import type { Snippet } from "svelte";
	import type { MouseEventHandler } from "svelte/elements";

    interface Props {
        href?: string,
        type?: 'link' | 'button' | 'text',
        onclick?: MouseEventHandler<HTMLButtonElement>,
        active?: boolean,
        disabled?: boolean,
        children: Snippet
    }

    const {
        href,
        type = 'link',
        active,
        disabled,
        onclick,
        children
    }: Props = $props();
</script>

<li>
    {#if type === 'button'}
        <button class="dropdown-item" class:active class:disabled {onclick}>
            {@render children()}
        </button>
    {:else if type === 'link'}
        <a class="dropdown-item" class:active class:disabled {href}>
            {@render children()}
        </a>
    {:else if type === 'text'}
        <span class="dropdown-item-text">
            {@render children()}
        </span>
    {/if}
</li>