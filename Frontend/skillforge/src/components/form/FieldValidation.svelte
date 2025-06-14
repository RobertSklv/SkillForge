<script lang="ts">
	import type FormContext from '$lib/types/FormContext';
	import type ValidationRule from '$lib/types/ValidationRule';
	import { getContext, onMount, type Snippet } from 'svelte';

    interface Props {
        name: string,
        label: string,
        value: any,
        isValid: boolean,
        shouldValidate: boolean,
        validateTogether?: string[],
    }

    var {
        name,
        label,
        value,
        isValid = $bindable(),
        shouldValidate,
        validateTogether,
    }: Props = $props();

    let errorMessages = $state<string[]>([]);
    let isInvalid = $derived<boolean>(!!errorMessages.length);

    $effect(() => {
        isValid = !isInvalid;
    });

    const formContext = getContext<FormContext>('form');

    export function validate(onlyVisited: boolean = true): string[] | null {
        if (onlyVisited && !shouldValidate) {
            return null;
        }

        if (!formContext) {
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
                        formContext.updateFieldValidation(name, errorMessages);
                    }
                })
            } else if (!isValid) {
                errorMessages.push(rule.message(label));
            }
        }

        if (validateTogether) {
            formContext.validateFields(validateTogether);
        }

        formContext.updateFieldValidation(name, errorMessages);

        return errorMessages;
    }

    function setErrors(errors: string[]) {
        errorMessages = errors;
    }

    onMount(() => {
        formContext?.registerFieldValidation(name, validate, setErrors);
    });
</script>

<div class="invalid-feedback">
    {#if isInvalid}
        {errorMessages[0]}
    {/if}
</div>