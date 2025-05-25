<script lang="ts" generics="T">
	import type FormContext from '$lib/types/FormContext';
	import type { InputType } from '$lib/types/InputType';
	import type ValidationRule from '$lib/types/ValidationRule';
	import { getContext, onMount } from 'svelte';

    interface Props {
        id: string,
        name?: string,
        label: string,
        type?: InputType,
        value: T,
        placeholder?: string,
        disabled?: boolean,
    }

    var {
        id,
        name = id,
        label,
        type = 'text',
        value = $bindable(),
        placeholder,
        disabled,
    }: Props = $props();

    let errorMessages = $state<string[]>([]);
    let isInvalid = $derived<boolean>(!!errorMessages.length);

    const formContext = getContext<FormContext>('form');

    function validate(): string[] {
        errorMessages = [];
        let rules: ValidationRule[] = formContext.getValidationRules(name);

        for (let rule of rules) {
            let isValid = rule.validate(value);

            if (!isValid) {
                errorMessages.push(rule.message(label));
            }
        }

        return errorMessages;
    }

    function setErrors(errors: string[]) {
        errorMessages = errors;
    }

    function oninput() {
        errorMessages = validate();

        formContext.updateField(name, errorMessages);
    }

    onMount(() => {
        formContext.registerField(name, validate, setErrors);
    });
</script>

<div class="mb-3">
    <label for={id} class="form-label">{label}:</label>
    <input {id}
            {type}
            {name}
            {placeholder}
            class="form-control"
            class:is-invalid={isInvalid}
            bind:value
            {oninput}
            {disabled}>
    {#if isInvalid}
        <div class="invalid-feedback">
            {errorMessages[0]}
        </div>
    {/if}
</div>