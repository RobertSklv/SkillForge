<script lang="ts" generics="T">
	import { goto } from '$app/navigation';
	import { createURLSearchParams } from '$lib/api/client';
	import type GridContext from '$lib/types/GridContext';
	import type GridState from '$lib/types/GridState';
	import type { GutterLevel } from '$lib/types/GutterLevel';
	import type PaginationResponse from '$lib/types/PaginationResponse';
	import { setContext, type Snippet } from 'svelte';
	import { writable, type Writable } from 'svelte/store';

	interface Props {
		gridState: GridState;
		defaultState: GridState;
		data: PaginationResponse<T>;
		url: string,
		cols: number;
		mdCols: number;
		lgCols: number;
		gap?: GutterLevel;
		item: Snippet<[T]>;
		header?: Snippet;
		footer?: Snippet;
	}

	let {
		gridState,
		defaultState,
		data,
		url,
		cols,
		mdCols,
		lgCols,
		gap,
		item,
		header,
		footer
	}: Props = $props();

	let colClasses = $derived(
		`col-${getColumnSize(cols)} col-md-${getColumnSize(mdCols)} col-lg-${getColumnSize(lgCols)}`
	);

	let gridStateWithDefaults = $derived<GridState>({
		p: gridState.p ?? defaultState.p,
		limit: gridState.limit ?? defaultState.limit,
		q: gridState.q ?? defaultState.q,
		sortBy: gridState.sortBy ?? defaultState.sortBy,
		sortOrder: gridState.sortOrder ?? defaultState.sortOrder,
	});

	let gridStateStore = writable<GridState>(gridStateWithDefaults);

	setContext<GridContext>('grid_context', {
		generateSearchUrl,
		updateState,
	});
	setContext<Writable<GridState>>('grid_state_store', gridStateStore);

	$effect(() => {
		gridStateStore.set(gridStateWithDefaults);
	})

	function getColumnSize(columnsCount: number): number {
		return 12 / columnsCount;
	}

	function getUpdatedState(param: string, value: any): GridState {
		let stateCopy = JSON.parse(JSON.stringify(gridState));
		stateCopy[param] = value;

		if (stateCopy.p === defaultState.p) {
			delete stateCopy.p;
		}
		if (stateCopy.limit === defaultState.limit) {
			delete stateCopy.limit;
		}
		if (stateCopy.q === defaultState.q) {
			delete stateCopy.q;
		}
		if (stateCopy.sortBy === defaultState.sortBy) {
			delete stateCopy.sortBy;
		}
		if (stateCopy.sortOrder === defaultState.sortOrder) {
			delete stateCopy.sortOrder;
		}

		return stateCopy;
	}

	export async function updateState(param: string, value: any): Promise<void> {
		let searchUrl = generateSearchUrl(param, value);

		await goto(searchUrl, {
			replaceState: false,
			keepFocus: true,
			noScroll: true
		});
	}

	export function generateSearchUrl(param: string, value: any): string {

		let updatedState = getUpdatedState(param, value);
		let query = createURLSearchParams(updatedState).toString();

		if (!url.startsWith('/')) {
			url = '/' + url;
		}

		return url + '?' + query;
	}
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
