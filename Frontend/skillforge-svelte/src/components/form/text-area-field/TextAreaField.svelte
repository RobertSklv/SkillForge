<script lang="ts">
	import type ValidatedField from '$lib/types/ValidatedField';
	import FieldValidation from '../field-validation/FieldValidation.svelte';
	import { getContext, onMount } from 'svelte';
	import type FormContext from '$lib/types/FormContext';

    interface Props {
        id: string,
        name?: string,
        label?: string,
        value: string | undefined | null,
        placeholder?: string,
        rows?: any,
        cols?: any,
        minlength?: any,
        maxlength?: any,
        mod?: string,
        disabled?: boolean,
        validateTogether?: string[],
    }

    var {
        id,
        name = id,
        label = name,
        value = $bindable(),
        placeholder,
        rows,
        cols,
        minlength,
        maxlength,
        mod,
        disabled,
        validateTogether,
    }: Props = $props();

    const formContext = getContext<FormContext>('form');

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

    export function reset() {
        value = formContext?.getFieldDefaultValue(name);
    }

    onMount(() => {
        formContext?.registerField(name, reset);
    })
</script>

<div class="mb-4">
    <label for={id} class="form-label">{label}:</label>
    <textarea {id}
            {name}
            {placeholder}
            class="form-control {mod}"
            class:is-invalid={!isValid}
            bind:value
            {rows}
            {cols}
            {minlength}
            {maxlength}
            {oninput}
            {onfocus}
            {onfocusout}
            {disabled}></textarea>
    <FieldValidation {name} {label} {value} shouldValidate={isVisited} bind:isValid bind:this={fieldValidation} {validateTogether} />
</div>