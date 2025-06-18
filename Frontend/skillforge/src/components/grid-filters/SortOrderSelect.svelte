<script lang="ts">
	import Select from "$components/form/Select.svelte";
	import type GridContext from "$lib/types/GridContext";
	import type GridState from "$lib/types/GridState";
	import type OptionType from "$lib/types/OptionType";
	import { getContext } from "svelte";
	import type { Writable } from "svelte/store";

	let gridStateStore = getContext<Writable<GridState>>('grid_state_store');
	let gridContext = getContext<GridContext>('grid_context');

	let sortOptions: OptionType[] = [
		{
			Value: 'desc',
			Label: 'Descending'
		},
		{
			Value: 'asc',
			Label: 'Ascending'
		},
	];

	function onValueChange(value: any) {
		gridContext.updateState('sortOrder', value);
	}
</script>

<Select id="sort_order" value={$gridStateStore.sortOrder} {onValueChange} options={sortOptions} />
