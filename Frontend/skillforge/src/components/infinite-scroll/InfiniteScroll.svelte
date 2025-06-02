<script lang="ts" generics="T">
	import { onDestroy, onMount, type Snippet } from 'svelte';

	interface Props {
		mod?: string,
		batchSize: number;
		autoLoadOnMount?: boolean,
		scrollableElement: () => HTMLElement,
        loadMore: (batchIndex: number) => Promise<T[]>,
		itemSnippet: Snippet<[T]>,
		placeholderSnippet?: Snippet,
	}

	let {
		mod,
        batchSize,
		autoLoadOnMount,
		scrollableElement,
        loadMore,
        itemSnippet,
		placeholderSnippet,
    }: Props = $props();

	let items = $state<T[]>([]);
	let batchIndex = $state<number>(0);
	let outOfItems = $state<boolean>(false);
	let containerElement: HTMLElement;
	let isLoading = $state<boolean>(false);

	export function updateItemList(newItems: T[]) {
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

		if (containerElement.getBoundingClientRect().bottom <= scrollableElement().clientHeight) {
            await loadMoreAndUpdateItemList();
		}
	}

	onMount(async () => {
		window.addEventListener('scroll', onScroll);

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

<div bind:this={containerElement} class={mod}>
	{#each items as item}
		{@render itemSnippet(item)}
	{/each}
	{#if isLoading}
		{@render placeholderSnippet?.()}
	{/if}
</div>
