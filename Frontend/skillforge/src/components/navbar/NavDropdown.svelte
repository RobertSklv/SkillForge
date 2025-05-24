<script lang="ts">
	import type { Snippet } from "svelte";

    const {
        buttonSnippet,
        children
    }: {
        buttonSnippet: Snippet,
        children: Snippet
    } = $props();

    let isOpen = $state(false);

    function onclick() {
        isOpen = !isOpen;
    }
</script>

<li class="nav-item dropdown">
    <button class="nav-link dropdown-toggle" {onclick} type="button" aria-expanded="false">
        {@render buttonSnippet()}
    </button>
    <ul class="dropdown-menu" class:show={isOpen}>
        {@render children()}
    </ul>
</li>