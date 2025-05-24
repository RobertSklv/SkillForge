<script lang="ts">
	import type { Snippet } from "svelte";

    const {
        href,
        isActive,
        children
    }: {
        href: string,
        isActive: boolean,
        children: Snippet
    } = $props();
</script>

<li class="nav-item">
    <a class="nav-link" class:active={isActive} {href}>
        {@render children()}
    </a>
</li>