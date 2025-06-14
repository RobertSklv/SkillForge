<script lang="ts">
	import ArticleCard from '$components/article/ArticleCard.svelte';
	import SortBySelect from '$components/grid-filters/SortBySelect.svelte';
	import SortOrderSelect from '$components/grid-filters/SortOrderSelect.svelte';
	import Grid from '$components/grid/Grid.svelte';
	import SearchBar from '$components/header/SearchBar.svelte';
	import Pagination from '$components/pagination/Pagination.svelte';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import type GridState from '$lib/types/GridState';
	import type PaginationResponse from '$lib/types/PaginationResponse';

	const DEFAULT_LIMIT = 9;

	interface Props {
		data: {
			response: PaginationResponse<ArticleCardType>;
			gridState: GridState;
		};
	}

	let { data }: Props = $props();

	let defaultGridState: GridState = {
		p: 1,
		limit: DEFAULT_LIMIT,
		q: undefined,
		sortBy: 'date',
		sortOrder: 'desc'
	};
</script>

<div class="mb-5">
	<h1>Search results for '{data.gridState?.q}':</h1>
	<p class="text-body-tertiary h5">Found: {data.response.TotalItems}</p>
</div>

<Grid
	gridState={data.gridState}
	defaultState={defaultGridState}
	data={data.response}
	url="/search"
	cols={1}
	mdCols={2}
	lgCols={3}
	gap={3}
>
	{#snippet header()}

		<div class="d-flex justify-content-center gap-5 mb-5">
			<SearchBar enableSuggestions={false} />

			<SortBySelect />

			<SortOrderSelect />
		</div>
		{#if data.response.TotalItems > DEFAULT_LIMIT}
			<div class="d-flex justify-content-center mb-3">
				<Pagination totalItems={data.response.TotalItems} defaultLimit={DEFAULT_LIMIT} />
			</div>
		{/if}
	{/snippet}

	{#snippet item(item)}
		<ArticleCard data={item} showComments={false} mod="h-100" />
	{/snippet}

	{#snippet footer()}
		{#if data.response.TotalItems > DEFAULT_LIMIT}
			<div class="d-flex justify-content-center mt-3">
				<Pagination totalItems={data.response.TotalItems} defaultLimit={DEFAULT_LIMIT} />
			</div>
		{/if}
	{/snippet}
</Grid>
