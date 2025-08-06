<script lang="ts">
	import type { BootstrapColor } from '$lib/types/BootstrapColor';
	import type TagLink from '$lib/types/TagLinkType';
	import Icon from '$components/icon/Icon.svelte';
	import Link from '../link/Link.svelte';
	import './_tag-link.scss';

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

<Link href="/tag/{data.Name}" {size} {background} {color} borderRadius={3} mod="tag-link" {muted}>
	<div class="d-flex tag-link">
		<Icon type="hash" />
		<span class="text-truncate">{data.Name}</span>
	</div>
</Link>