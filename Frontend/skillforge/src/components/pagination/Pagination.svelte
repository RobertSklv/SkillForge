<script lang="ts">
	import type GridState from '$lib/types/GridState';
	import { clamp } from '$lib/util';
	import { getContext } from 'svelte';
	import type { Writable } from 'svelte/store';

	const INNER_PAGINATION_LINKS_COUNT = 9;

	interface Props {
		totalItems: number;
		defaultLimit: number;
	}

	let { totalItems, defaultLimit }: Props = $props();

	let gridStateStore = getContext<Writable<GridState>>('grid_state');

	let currentPage = $derived<number>($gridStateStore.p ?? 1);
	let totalPages = $derived<number>(
		Math.ceil(totalItems / ($gridStateStore.limit ?? defaultLimit))
	);
	let totalLinks = $derived<number>(
		totalPages > INNER_PAGINATION_LINKS_COUNT ? INNER_PAGINATION_LINKS_COUNT : totalPages
	);
	let offsetStart = $derived<number>(
		clamp(
			currentPage - INNER_PAGINATION_LINKS_COUNT / 2,
			1,
			totalPages - INNER_PAGINATION_LINKS_COUNT + 1
		)
	);

	function setPage(
		e: MouseEvent & { currentTarget: EventTarget & HTMLAnchorElement },
		page: number
	) {
		e.preventDefault();
		$gridStateStore.p = page;
	}
</script>

<nav aria-label="Pagination">
	<ul class="pagination">
		<li class="page-item">
			<a
				class="page-link"
				href="/"
				class:disabled={currentPage === 1}
				tabindex={currentPage === 1 ? -1 : undefined}
				onclick={(e) => setPage(e, currentPage - 1)}>Previous</a
			>
		</li>

		{#each { length: totalLinks } as _, i}
			{@const pageIndex = i + 1}
			{@const page = offsetStart + i}

			<li class="page-item">
				<a
					class="page-link"
					href="?p={pageIndex}"
					class:active={page === currentPage}
					aria-current={page === currentPage ? 'page' : undefined}
					onclick={(e) => setPage(e, pageIndex)}
				>
					{#if (pageIndex == 2 && page > 2) || (pageIndex == INNER_PAGINATION_LINKS_COUNT - 1 && page < totalPages - 1)}
						...
					{:else}
						{pageIndex}
					{/if}
				</a>
			</li>
		{/each}

		<li class="page-item">
			<a
				class="page-link"
				href="/"
				class:disabled={currentPage === totalLinks}
				tabindex={currentPage === totalLinks ? -1 : undefined}
				onclick={(e) => setPage(e, currentPage + 1)}>Next</a
			>
		</li>
	</ul>
</nav>
