<script lang="ts">
	import type ValidatedField from '$lib/types/ValidatedField';
	import type { InputType } from '$lib/types/InputType';
	import FieldValidation from './FieldValidation.svelte';

    interface Props {
        id: string,
        name?: string,
        label?: string,
        type?: InputType,
        value: any,
        placeholder?: string,
        disabled?: boolean,
        validateTogether?: string[],
    }

    var {
        id,
        name = id,
        label = name,
        type = 'text',
        value = $bindable(),
        placeholder,
        disabled,
        validateTogether,
    }: Props = $props();

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);

    let fieldValidation: ValidatedField;
    function oninput() {
        fieldValidation?.validate();
    }

    function onfocus() {
        isVisited = true;
    }

    function onfocusout() {
        fieldValidation?.validate();
    }
</script>

<div class="mb-3">
    <label for={id} class="form-label">{label}:</label>
    <input {id}
            {type}
            {name}
            {placeholder}
            class="form-control"
            class:is-invalid={!isValid}
            bind:value
            {oninput}
            {onfocus}
            {onfocusout}
            {disabled}>
    <FieldValidation {name} {label} {value} shouldValidate={isVisited} bind:isValid bind:this={fieldValidation} {validateTogether} />
</div>