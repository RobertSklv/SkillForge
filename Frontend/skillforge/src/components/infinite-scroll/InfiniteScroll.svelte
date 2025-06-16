<script lang="ts" generics="T">
	import type { GutterLevel } from '$lib/types/GutterLevel';
	import type ScrollableStore from '$lib/types/ScrollableStore';
	import { getContext, onDestroy, onMount, type Snippet } from 'svelte';

	interface Props {
		mod?: string;
		batchSize: number;
		autoLoadOnMount?: boolean;
		preloadedBatches?: [T[]];
		listDirection?: 'row' | 'column',
		gap?: GutterLevel,
		loadMore: (batchIndex: number) => Promise<T[]>;
		itemSnippet: Snippet<[T]>;
		placeholderSnippet?: Snippet;
		outOfItemsSnippet?: Snippet;
	}

	let {
		mod,
		batchSize,
		autoLoadOnMount,
		preloadedBatches,
		listDirection = 'column',
		gap = 3,
		loadMore,
		itemSnippet,
		placeholderSnippet,
		outOfItemsSnippet
	}: Props = $props();

	let items = $state<T[]>(preloadedBatches?.flat() ?? []);
	let batchIndex = $state<number>(preloadedBatches?.length ?? 0);
	let outOfItems = $state<boolean>(false);
	let containerElement: HTMLElement;
	let isLoading = $state<boolean>(false);

	const scrollable = getContext<ScrollableStore>('scrollable');

	function getScrollableHeight() {
		if (
			scrollable &&
			typeof scrollable.getClientHeight !== 'undefined' &&
			scrollable.getClientHeight != null
		) {
			return scrollable.getClientHeight();
		}

		return document.documentElement.clientHeight;
	}

	function updateItemList(newItems: T[]) {
		items = items.concat(newItems);

		if (newItems.length < batchSize) {
			outOfItems = true;
		}

		batchIndex++;
	}

	async function loadMoreAndUpdateItemList() {
		if (isLoading) {
			return;
		}

		isLoading = true;

		let newItems = await loadMore(batchIndex);
		updateItemList(newItems);

		isLoading = false;
	}

	async function onScroll() {
		if (outOfItems) {
			return;
		}

		if (containerElement.getBoundingClientRect().bottom <= getScrollableHeight()) {
			await loadMoreAndUpdateItemList();
		}
	}

	onMount(async () => {
		if (scrollable?.onScroll) {
			scrollable.onScroll(onScroll);
		} else {
			window.addEventListener('scroll', onScroll);
		}

		if (autoLoadOnMount) {
			await loadMoreAndUpdateItemList();
		}
	});

	onDestroy(() => {
		if (typeof window !== 'undefined') {
			window.removeEventListener('scroll', onScroll);
		}
	});
</script>

<div bind:this={containerElement} class="d-flex flex-{listDirection} gap-{gap} {mod}">
	{#each items as item}
		{@render itemSnippet(item)}
	{/each}
	{#if outOfItems}
		{@render outOfItemsSnippet?.()}
	{/if}
	{#if isLoading}
		{@render placeholderSnippet?.()}
	{/if}
</div>
