<script lang="ts">
	import type { BootstrapColor } from 'skillforge-common/types/BootstrapColor';
	import LinkPlaceholder from '../link-placeholder/LinkPlaceholder.svelte';

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