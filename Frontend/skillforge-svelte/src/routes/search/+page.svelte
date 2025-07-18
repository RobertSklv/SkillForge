<script lang="ts">
	import ArticleCard from '$components/article-card/ArticleCard.svelte';
	import SortBySelect from '$components/grid/filters/sort-by-select/SortBySelect.svelte';
	import SortOrderSelect from '$components/grid/filters/sort-order-select/SortOrderSelect.svelte';
	import Grid from '$components/grid/Grid.svelte';
	import SearchBar from '$components/grid/filters/search-bar/SearchBar.svelte';
	import Pagination from '$components/pagination/Pagination.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import type ArticleCardType from 'skillforge-common/types/ArticleCardType';
	import type GridState from 'skillforge-common/types/GridState';
	import type PaginationResponse from 'skillforge-common/types/PaginationResponse';

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

<svelte:head>
	<title>SkillForge | Search: '{data.gridState?.q}'</title>
	<meta name="description" content="Search results for: '{data.gridState?.q}'" />
	<meta name="robots" content="noindex,nofollow" />
	<link rel="canonical" href="{PUBLIC_BASE_URL}/search" />
</svelte:head>

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
		<div class="row mb-5">
			<div class="col-12 col-md-6 mb-4 mb-md-0">
				<SearchBar />
			</div>

			<div class="col-6 col-md-3">
				<SortBySelect />
			</div>

			<div class="col-6 col-md-3">
				<SortOrderSelect />
			</div>
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
