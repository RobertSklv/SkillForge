<script lang="ts">
	import { clickOutside } from "$lib/util";
	import { type Snippet } from "svelte";

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
    
    function closeDropdown() {
        isOpen = false;
    }
</script>

<li class="nav-item dropdown">
    <button class="nav-link dropdown-toggle" {onclick} type="button" aria-expanded="false" use:clickOutside={closeDropdown}>
        {@render buttonSnippet()}
    </button>
    <ul class="dropdown-menu" class:show={isOpen}>
        {@render children()}
    </ul>
</li>