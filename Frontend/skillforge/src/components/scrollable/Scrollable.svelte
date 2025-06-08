<script lang="ts">
	import type ScrollableStore from '$lib/types/ScrollableStore';
	import { setContext } from 'svelte';
	import { writable } from 'svelte/store';

	let props = $props();

	let scrollableElement: HTMLElement;
	let onScrollCallbacks: (() => void)[] = [];

	let scrollableStore = writable<ScrollableStore>({
		getClientHeight: () => {
			return scrollableElement?.getBoundingClientRect().top + scrollableElement?.clientHeight;
		},
		onScroll: (callback: () => void) => {
			onScrollCallbacks.push(callback);
		}
	});

	function onscroll() {
		onScrollCallbacks?.forEach((cb) => cb());
	}

	setContext<ScrollableStore>('scrollable', $scrollableStore);
</script>

<div {...props} bind:this={scrollableElement} {onscroll}>
	{@render props.children?.()}
</div>
