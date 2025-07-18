<script lang="ts">
	import { goto } from '$app/navigation';
	import { searchArticles } from 'skillforge-common/api/client';
	import type ArticleSearchItemType from 'skillforge-common/types/ArticleSearchItemType';
	import type GridContext from 'skillforge-common/types/GridContext';
	import moment from 'moment';
	import { getContext } from 'svelte';
	import { fade } from 'svelte/transition';

	interface Props {
		mod?: string;
	}

	let {
		mod,
	}: Props = $props();

	let inputValue = $state('');

	let gridContext = getContext<GridContext>('grid_context');

	function search() {
        gridContext.updateState('q', inputValue);
	}

	function onkeydown(e: KeyboardEvent & { currentTarget: EventTarget & HTMLInputElement }) {
		if (e.code === 'Enter') {
			if (inputValue.length) {
				search();
			}
		}
	}
</script>

<div class="input-group">
    <input
        id="search"
        type="search"
        class="form-control rounded-start-3"
        placeholder="Search articles..."
        aria-label="Search articles..."
        aria-describedby="search_button"
        autocomplete="off"
        {onkeydown}
        bind:value={inputValue}
    />
    <button class="btn btn-light rounded-end-3" type="button" id="search_button" onclick={search}>Search</button>
</div>