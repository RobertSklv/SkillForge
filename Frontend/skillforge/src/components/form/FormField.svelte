<script lang="ts" generics="T">
	import type FormContext from '$lib/types/FormContext';
	import type FormFieldType from '$lib/types/FormFieldType';
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
        validateTogether?: FormFieldType[]
    }

    var {
        id,
        name = id,
        label,
        type = 'text',
        value = $bindable(),
        placeholder,
        disabled,
        validateTogether,
    }: Props = $props();

    let errorMessages = $state<string[]>([]);
    let isInvalid = $derived<boolean>(!!errorMessages.length);
    let isVisited = $state<boolean>();

    const formContext = getContext<FormContext>('form');

    export function validate(onlyVisited: boolean = true): string[] | null {
        if (onlyVisited && !isVisited) {
            return null;
        }

        errorMessages = [];
        let rules: ValidationRule[] = formContext.getValidationRules(name);

        for (let rule of rules) {
            let isValid = rule.validate(value);

            if (isValid instanceof Promise) {
                isValid.then(r => {
                    if (!r) {
                        errorMessages.push(rule.message(label));
                        formContext.updateField(name, errorMessages);
                    }
                })
            } else if (!isValid) {
                errorMessages.push(rule.message(label));
            }
        }

        if (validateTogether) {
            for (let field of validateTogether) {
                field.validate(true);
            }
        }

        return errorMessages;
    }

    function setErrors(errors: string[]) {
        errorMessages = errors;
    }

    function oninput() {
        let errors = validate();

        if (errors != null) {
            errorMessages = errors;
        }

        formContext.updateField(name, errorMessages);
    }

    function onfocus() {
        isVisited = true;
    }

    function onfocusout() {
        validate();
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
            {onfocus}
            {onfocusout}
            {disabled}>
    <div class="invalid-feedback">
        {#if isInvalid}
            {errorMessages[0]}
        {/if}
    </div>
</div>