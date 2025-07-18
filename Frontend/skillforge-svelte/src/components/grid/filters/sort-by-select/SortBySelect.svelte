<script lang="ts">
	import Select from "$components/form/select/Select.svelte";
	import type GridContext from "skillforge-common/types/GridContext";
	import type GridState from "skillforge-common/types/GridState";
	import type OptionType from "skillforge-common/types/OptionType";
	import { getContext } from "svelte";
	import type { Writable } from "svelte/store";

	let gridStateStore = getContext<Writable<GridState>>('grid_state_store');
	let gridContext = getContext<GridContext>('grid_context');

	let sortOptions: OptionType[] = [
		{
			Value: 'date',
			Label: 'Date published'
		},
		{
			Value: 'rating',
			Label: 'Rating'
		},
		{
			Value: 'views',
			Label: 'Views'
		}
	];

	function onValueChange(value: any) {
		gridContext.updateState('sortBy', value);
	}
</script>

<Select id="sort_by" value={$gridStateStore.sortBy} {onValueChange} options={sortOptions} />
