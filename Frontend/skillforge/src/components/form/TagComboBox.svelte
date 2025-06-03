<script lang="ts">
	import { searchTags } from '$lib/api/client';
	import type TagLinkType from '$lib/types/TagLinkType';
	import type ValidatedField from '$lib/types/ValidatedField';
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
	let fieldValidation: ValidatedField;

	let inputValue = $state('');
	let showSuggestions = $state(false);
	let suggestions = $state<TagLinkType[]>([]);
	let isWithinLimit = $derived(!tags || tags.length < limit);
	let isValid = $state<boolean>(true);

	async function updateSuggestions() {
		if (isWithinLimit) {
			suggestions = await searchTags(inputValue, tags);

			if (inputValue && !suggestions.map((s) => s.Name).includes(inputValue)) {
				suggestions.unshift({
					Name: inputValue
				});
			}
		}
	}

	function oninput() {
		fieldValidation?.validate();
		updateSuggestions();
	}

	function onfocus() {
		showSuggestions = true;
		updateSuggestions();
	}

	function onfocusout(e: FocusEvent & { currentTarget: EventTarget & HTMLInputElement }) {
		fieldValidation?.validate();
		if (!suggestionsElement?.contains(e.relatedTarget as Node)) {
			showSuggestions = false;
		}
	}

	function onkeydown(e: KeyboardEvent & { currentTarget: EventTarget & HTMLInputElement }) {
		if (e.code === 'Enter') {
			if (inputValue.length) {
				addTag(inputValue);
			}
		} else if (e.code === 'Backspace') {
			if (!inputValue.length) {
				let lastTag = tags.pop();

				if (lastTag) {
					// The tag is already removed at this point, this removeTag() call is needed to update the state,
					// because tags.pop() does not update the component state
					removeTag(lastTag);

					inputValue = lastTag;
					updateSuggestions();
				}
			}
		}
	}

	function addTag(tagName: string) {
		tags ??= [];

		if (!isWithinLimit || !isValid) {
			return;
		}

		if (!tags.includes(tagName)) {
			tags.push(tagName);
		}

		inputValue = '';
		showSuggestions = false;
		updateSuggestions();
		fieldValidation?.validate();
	}

	function removeTag(tagName: string) {
		tags = tags.filter((t) => t != tagName);

		updateSuggestions();
		fieldValidation?.validate();
	}
</script>

<div class="combo-box position-relative mb-4">
	<label for={id} class="form-label">{label}</label>
	<div class="d-flex flex-wrap gap-1" class:is-invalid={!isValid}>
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
			{onkeydown}
			class:is-invalid={!isValid}
			autocomplete="off"
			bind:value={inputValue}
		/>
	</div>
	{#if showSuggestions && suggestions.length && isWithinLimit && isValid}
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
	<FieldValidation
		name={id}
		{label}
		value={inputValue ? tags.concat(inputValue) : tags}
		shouldValidate={true}
		bind:isValid
		bind:this={fieldValidation}
	/>
</div>
