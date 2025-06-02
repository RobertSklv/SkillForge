<script lang="ts">
	import { searchTags } from '$lib/api/client';
	import type TagLinkType from '$lib/types/TagLinkType';
	import FieldValidation from './FieldValidation.svelte';

	interface Props {
		id: string;
		label?: string;
		tags: string[];
		limit?: number;
	}

	let {
        id,
        label = id,
        tags = $bindable([]),
        limit = 3
    }: Props = $props();

	let suggestionsElement: HTMLElement;

	let inputValue = $state('');
	let showSuggestions = $state(false);
	let suggestions = $state<TagLinkType[]>([]);
	let isWithinLimit = $derived(!tags || tags.length < limit);
	let isValid = $state<boolean>(true);

	async function updateSuggestions() {
		if (isWithinLimit) {
			suggestions = await searchTags(inputValue, tags);
		}
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
			showSuggestions = false;
		}
	}

	function addTag(tagName: string) {
    tags ??= [];

		if (!tags.includes(tagName)) {
			tags.push(tagName);
		}

		inputValue = '';
		showSuggestions = false;
		updateSuggestions();
	}

	function removeTag(tagName: string) {
		tags = tags.filter((t) => t != tagName);

		updateSuggestions();
	}
</script>

<div class="combo-box position-relative">
	<label for={id} class="form-label">{label}</label>
	<div class="d-flex flex-wrap gap-1">
		{#each tags as tag}
			<div class="toast show w-auto">
				<div class="d-flex">
					<div class="toast-body p-2">
						<strong>#{tag}</strong>
					</div>
					<button
						type="button"
						class="btn-close me-2 m-auto"
						aria-label="Close"
						onclick={() => removeTag(tag)}
					></button>
				</div>
			</div>
		{/each}
		<input
			{id}
			type="text"
			class="form-control flex-grow-1 w-auto"
			{onfocus}
			{onfocusout}
			{oninput}
			class:is-invalid={!isValid}
            autocomplete="off"
			bind:value={inputValue}
		/>
	</div>
	{#if showSuggestions && suggestions.length && isWithinLimit}
		<div class="position-absolute w-100 z-3 shadow" bind:this={suggestionsElement}>
			<ul class="list-group">
				{#each suggestions as tag}
					<button
						type="button"
						onclick={() => addTag(tag.Name)}
						class="list-group-item list-group-item-action d-flex justify-content-between align-items-start"
					>
						<div class="ms-2 me-auto">
							<div class="fw-bold" style="margin-left: -4px;">
								<i class="bi bi-hash"></i>{tag.Name}
							</div>
							<small class="text-body-tertiary">{tag.Description}</small>
						</div>
					</button>
				{/each}
			</ul>
		</div>
	{/if}
	<FieldValidation name={id} {label} value={tags} shouldValidate={true} bind:isValid />
</div>
