<script lang="ts" generics="T">
	import type ScrollableStore from '$lib/types/ScrollableStore';
	import { getContext, onDestroy, onMount, type Snippet } from 'svelte';

	interface Props {
		mod?: string,
		batchSize: number;
        loadMore: (batchIndex: number) => Promise<T[]>,
		itemSnippet: Snippet<[T]>,
		placeholderSnippet?: Snippet,
	}

	let {
		mod,
        batchSize,
        loadMore,
        itemSnippet,
		placeholderSnippet,
    }: Props = $props();

	let items = $state<T[]>([]);
	let batchIndex = $state<number>(0);
	let outOfItems = $state<boolean>(false);
	let containerElement: HTMLElement;
	let isLoading = $state<boolean>(false);

	const scrollable = getContext<ScrollableStore>('scrollable');

	function getScrollableHeight() {
		if (scrollable && typeof scrollable.getClientHeight !== 'undefined' && scrollable.getClientHeight != null) {
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

		await loadMoreAndUpdateItemList();
	});

	onDestroy(() => {
        if (typeof window !== 'undefined') {
		    window.removeEventListener('scroll', onScroll);
        }
	});
</script>

<div bind:this={containerElement} class={mod}>
	{#each items as item}
		{@render itemSnippet(item)}
	{/each}
	{#if isLoading}
		{@render placeholderSnippet?.()}
	{/if}
</div>
