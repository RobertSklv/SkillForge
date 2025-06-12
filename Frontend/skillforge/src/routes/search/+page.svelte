<script lang="ts">
	import ArticleCard from '$components/article/ArticleCard.svelte';
	import Grid from '$components/grid/Grid.svelte';
	import Pagination from '$components/pagination/Pagination.svelte';
	import { searchArticlesAdvanced } from '$lib/api/client';
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

	let gridState = $state<GridState>(data.gridState);

	function updateGridState(state: GridState) {
		return searchArticlesAdvanced(state);
	}
</script>

<div class="mb-5">
    <h1>Search results for '{gridState?.q}':</h1>
    <p class="text-body-tertiary h5">Found: {data.response.TotalItems}</p>
</div>

<Grid
	{gridState}
	initialData={data.response}
	cols={1}
	mdCols={2}
	lgCols={3}
	gap={3}
	update={updateGridState}
>
	{#snippet header()}
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
