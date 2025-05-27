<script lang="ts">
	import type ValidatedField from '$lib/types/ValidatedField';
	import { setContext, type Snippet } from 'svelte';
	import FieldValidation from './FieldValidation.svelte';
	import type SelectFieldContext from '$lib/types/SelectFieldContext';

    interface Props {
        id: string,
        name?: string,
        label?: string,
        value: any,
        size?: number,
        disabled?: boolean,
        multiple?: boolean,
        validateTogether?: string[],
        children: Snippet,
    }

    interface Options {
        [key: string]: {
            setSelected: (isSelected: boolean) => void,
        }
    }

    var {
        id,
        name = id,
        label = name,
        value = $bindable(),
        size,
        disabled,
        multiple,
        validateTogether,
        children,
    }: Props = $props();

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);
    let fieldValidation: ValidatedField;
    let options: Options = {};

    $effect(() => {
        let valueArray: any[] = multiple ? value : [value];

        for (let optionKey in options) {
            options[optionKey].setSelected(valueArray.includes(optionKey));
        }
    })

    setContext<SelectFieldContext>('select', {
        registerField,
    });
    
    function registerField(name: string, setSelected: (isSelected: boolean) => void) {
        options[name] = {
            setSelected
        }
    }

    function oninput(e: Event & { currentTarget: EventTarget & HTMLSelectElement; }) {
        if (e.target != null) {
            if (multiple) {
                value = Array.from((e.target as HTMLSelectElement).selectedOptions).map(option => option.value);
            } else {
                value = (e.target as HTMLSelectElement).value;
            }
        }

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
    <select {id}
            {name}
            {size}
            class="form-select"
            class:is-invalid={!isValid}
            {oninput}
            {onfocus}
            {onfocusout}
            {disabled}
            {multiple}>
        {@render children()}
    </select>
    <FieldValidation {name} {label} {value} shouldValidate={isVisited} bind:isValid bind:this={fieldValidation} {validateTogether} />
</div>