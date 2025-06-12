<script lang="ts" generics="T">
	import Pagination from '$components/pagination/Pagination.svelte';
	import type GridState from '$lib/types/GridState';
	import type { GutterLevel } from '$lib/types/GutterLevel';
	import type PaginationResponse from '$lib/types/PaginationResponse';
	import { onDestroy, onMount, setContext, type Snippet } from 'svelte';
	import { writable, type Unsubscriber, type Writable } from 'svelte/store';

	interface Props {
		gridState: GridState;
		initialData: PaginationResponse<T>;
		cols: number;
		mdCols: number;
		lgCols: number;
		gap?: GutterLevel;
		update: (gridState: GridState) => Promise<PaginationResponse<T>>;
		item: Snippet<[T]>;
		header?: Snippet;
		footer?: Snippet;
	}

	let {
		gridState,
		initialData,
		cols,
		mdCols,
		lgCols,
		gap,
		update,
		item,
		header,
		footer
	}: Props = $props();

	let data = $state<PaginationResponse<T>>(initialData);
	let colClasses = $derived(
		`col-${getColumnSize(cols)} col-md-${getColumnSize(mdCols)} col-lg-${getColumnSize(lgCols)}`
	);

	// setContext<GridState>('grid_state', gridState);
	let gridStateStore = writable<GridState>(gridState);
	setContext<Writable<GridState>>('grid_state', gridStateStore);

	let unsubscribeFromGridStateStore: Unsubscriber;

	function getColumnSize(columnsCount: number): number {
		return 12 / columnsCount;
	}

	onMount(() => {
		unsubscribeFromGridStateStore = gridStateStore.subscribe(async (newState) => {
			data = await update(newState);
		});
	});

	onDestroy(() => {
		unsubscribeFromGridStateStore?.();
	});
</script>

{@render header?.()}
<div class="row">
	{#each data.Items as i}
		<div class="{colClasses} p-{gap}">
			{@render item(i)}
		</div>
	{/each}
</div>
{@render footer?.()}
