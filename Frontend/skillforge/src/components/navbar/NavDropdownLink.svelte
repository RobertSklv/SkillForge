<script lang="ts">
	import type { Snippet } from "svelte";

    const {
        href,
        children
    }: {
        href: string,
        children: Snippet
    } = $props();
</script>

<li>
    <a class="dropdown-item" {href}>
        {@render children()}
    </a>
</li>