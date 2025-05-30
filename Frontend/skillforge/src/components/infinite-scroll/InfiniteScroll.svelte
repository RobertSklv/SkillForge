<script lang="ts" generics="T">
	import { onDestroy, onMount, type Snippet } from 'svelte';

	interface Props {
		batchSize: number;
        loadMore: (batchIndex: number) => Promise<T[]>,
		itemSnippet: Snippet<[T]>,
		placeholderSnippet?: Snippet,
	}

	let {
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

	async function loadMoreAndUpdateItemList() {
		if (isLoading) {
			return;
		}

		isLoading = true;

		let newItems = await loadMore(batchIndex);
		items = items.concat(newItems);

		if (newItems.length < batchSize) {
			outOfItems = true;
		}

		batchIndex++;
		isLoading = false;
	}

	async function onScroll() {
		if (outOfItems) {
			return;
		}

		if (containerElement.getBoundingClientRect().bottom <= document.documentElement.clientHeight) {
            await loadMoreAndUpdateItemList();
		}
	}

	onMount(async () => {
        console.log('add listener to', window);
		window.addEventListener('scroll', onScroll);

		await loadMoreAndUpdateItemList();
	});

	onDestroy(() => {
        if (typeof window !== 'undefined') {
            console.log('remove listener from', window);
		    window.removeEventListener('scroll', onScroll);
        }
	});
</script>

<div bind:this={containerElement}>
	{#each items as item}
		{@render itemSnippet(item)}
	{/each}
	{#if isLoading}
		{@render placeholderSnippet?.()}
	{/if}
</div>
