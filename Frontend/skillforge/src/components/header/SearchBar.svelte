<script lang="ts">
	import { goto } from '$app/navigation';
	import { searchArticles } from '$lib/api/client';
	import type ArticleSearchItemType from '$lib/types/ArticleSearchItemType';
	import type GridContext from '$lib/types/GridContext';
	import moment from 'moment';
	import { getContext } from 'svelte';
	import { fade } from 'svelte/transition';

	interface Props {
		enableSuggestions?: boolean
	}

	let {
		enableSuggestions = true
	}: Props = $props();

	let suggestionsElement: HTMLElement;

	let inputValue = $state('');
	let suggestions = $state<ArticleSearchItemType[]>([]);
	let showSuggestions = $state<boolean>(false);

	let gridContext = getContext<GridContext>('grid_context');

	async function updateSuggestions() {
		if (inputValue) {
			suggestions = await searchArticles(inputValue);
		} else {
			suggestions = [];
		}
	}

	function hideSuggestions() {
		showSuggestions = false;
	}

	function oninput() {
		updateSuggestions();
	}

	function onfocus() {
		showSuggestions = true;
		updateSuggestions();
	}

	function onfocusout(e: FocusEvent & { currentTarget: EventTarget & HTMLInputElement }) {
		if (!suggestionsElement?.contains(e.relatedTarget as Node)) {
			hideSuggestions();
		}
	}

	function search() {
		if (gridContext) {
			gridContext.updateState('q', inputValue);
		} else {
			goto(`/search?q=${encodeURIComponent(inputValue)}`);
		}
	}

	function onkeydown(e: KeyboardEvent & { currentTarget: EventTarget & HTMLInputElement }) {
		if (e.code === 'Enter') {
			if (inputValue.length) {
				search();
			}
		}
	}
</script>

<div class="position-relative w-50">
	<div class="input-group">
		<input
			id="search"
			type="search"
			class="form-control rounded-start-3"
			placeholder="Search articles..."
			aria-label="Search articles..."
			aria-describedby="search_button"
			autocomplete="off"
			{oninput}
			{onfocus}
			{onfocusout}
			{onkeydown}
			bind:value={inputValue}
		/>
		<button class="btn btn-light rounded-end-3" type="button" id="search_button" onclick={search}>Search</button>
	</div>
	{#if showSuggestions && enableSuggestions && suggestions.length}
		<div
			class="position-absolute w-100 z-3 mt-1 shadow"
			bind:this={suggestionsElement}
			transition:fade={{ duration: 100 }}
		>
			<ul class="list-group rounded-3">
				{#each suggestions as item}
					<a
						href="/article/{item.ArticleId}"
						onclick={hideSuggestions}
						class="list-group-item list-group-item-action d-flex justify-content-between align-items-start"
					>
						<div class="ms-2 me-auto">
							<h4 class="h6" style="margin-left: -4px;">
								{item.Title}
							</h4>
							<small class="text-body-tertiary">@{item.AuthorName}</small>
							<small class="ms-2 text-muted">{moment(item.DatePosted).format('MMM Do YY')}</small>
						</div>
					</a>
				{/each}
			</ul>
		</div>
	{/if}
</div>
