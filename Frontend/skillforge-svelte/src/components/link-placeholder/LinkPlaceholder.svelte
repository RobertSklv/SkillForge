<script lang="ts">
	import type { BootstrapColor } from 'skillforge-common/types/BootstrapColor';
	import type TagLink from 'skillforge-common/types/TagLinkType';
	import type { Snippet } from 'svelte';

	interface Props {
		size?: 'normal' | 'small';
		background?: 'outline' | 'fill';
		color?: BootstrapColor;
		borderRadius?: 0 | 1 | 2 | 3 | 4 | 5 | 'pill' | 'circle';
	}

	let {
		size = 'normal',
		background = 'outline',
		color = 'primary',
		borderRadius = 3,
	}: Props = $props();

	let _class = $derived(
		`placeholder col-5 ${size === 'normal' ? 'ps-2 py-1' : 'p-0 pe-1'} btn btn${background === 'outline' ? '-outline' : ''}-${color} rounded-${borderRadius} border-1 shadow-none`
	);
</script>

<span class={_class}></span>
