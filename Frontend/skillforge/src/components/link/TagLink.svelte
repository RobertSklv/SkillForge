<script lang="ts">
	import type { BootstrapColor } from '$lib/types/BootstrapColor';
	import type TagLink from '$lib/types/TagLinkType';
	import Link from './Link.svelte';

	interface Props {
		data: TagLink;
		size?: 'normal' | 'small';
		background?: 'outline' | 'fill';
		color?: BootstrapColor;
		muted?: boolean;
	}

	let {
		data,
		size,
		background,
		color,
		muted
	}: Props = $props();
</script>

<Link href="/tag/{data.Name}" {size} {background} {color} borderRadius={3} {muted}>
	<i class="bi bi-hash"></i>{data.Name}
</Link>