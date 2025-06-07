<script lang="ts">
	import type { BootstrapColor } from '$lib/types/BootstrapColor';
	import LinkPlaceholder from './LinkPlaceholder.svelte';

	interface Props {
		size?: 'normal' | 'small';
		background?: 'outline' | 'fill';
		color?: BootstrapColor;
	}

	let {
		size,
		background,
		color,
	}: Props = $props();
</script>

<LinkPlaceholder {size} {background} {color} borderRadius={3} />