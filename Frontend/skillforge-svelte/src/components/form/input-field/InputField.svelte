<script lang="ts">
	import type ValidatedField from '$lib/types/ValidatedField';
	import type { InputType } from '$lib/types/InputType';
	import FieldValidation from '../field-validation/FieldValidation.svelte';
	import { getContext, onMount } from 'svelte';
	import type FormContext from '$lib/types/FormContext';

    interface Props {
        id: string,
        name?: string,
        label?: string,
        type?: InputType,
        value: any,
        placeholder?: string,
        min?: any,
        max?: any,
        minlength?: any,
        maxlength?: any,
        step?: any,
        mod?: string,
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
        min,
        max,
        minlength,
        maxlength,
        step,
        mod,
        disabled,
        validateTogether,
    }: Props = $props();

    const formContext = getContext<FormContext>('form');

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);
    let formClass = $derived<string>(type === 'range' ? 'form-range' : 'form-control');

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
    <input {id}
            {type}
            {name}
            {placeholder}
            class="{formClass} {mod}"
            class:is-invalid={!isValid}
            bind:value
            {min}
            {max}
            {minlength}
            {maxlength}
            {step}
            {oninput}
            {onfocus}
            {onfocusout}
            {disabled}>
    <FieldValidation {name} {label} {value} shouldValidate={isVisited} bind:isValid bind:this={fieldValidation} {validateTogether} />
</div>