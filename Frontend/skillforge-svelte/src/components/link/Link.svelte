<script lang="ts">
	import type { BootstrapColor } from '$lib/types/BootstrapColor';
	import type TagLink from '$lib/types/TagLinkType';
	import type { Snippet } from 'svelte';

	interface Props {
		href: string;
		size?: 'normal' | 'small';
		background?: 'outline' | 'fill';
		mod?: string;
		color?: BootstrapColor;
		borderRadius?: 0 | 1 | 2 | 3 | 4 | 5 | 'pill' | 'circle';
		muted?: boolean;
		children: Snippet;
	}

	let {
		href,
		size = 'normal',
		background = 'outline',
		mod,
		color = 'primary',
		borderRadius = 3,
		muted,
		children
	}: Props = $props();

	let _class = $derived(
		`${mod} ${size === 'normal' ? 'ps-2 py-1' : 'p-0 pe-1'} btn btn${background === 'outline' ? '-outline' : ''}-${color} rounded-${borderRadius} border-1 shadow-none ${muted ? 'opacity-50' : ''}`
	);
</script>

<a {href} class={_class}>
	{@render children()}
</a>
