<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		href: string;
		active?: boolean;
		children: Snippet;
	}

	const {
        href,
        active,
        children
    }: Props = $props();
</script>

<li class="nav-item">
	<a class="nav-link" class:active {href}>
		{@render children()}
	</a>
</li>
