<script lang="ts">
	import type ValidatedField from '$lib/types/ValidatedField';
	import { setContext, type Snippet } from 'svelte';
	import FieldValidation from './FieldValidation.svelte';
	import type SelectFieldContext from '$lib/types/SelectFieldContext';
    import type OptionType from '$lib/types/Option';
	import Option from './Option.svelte';

    interface Props {
        id: string,
        name?: string,
        label?: string,
        value: any,
        size?: any,
        mod?: string,
        disabled?: boolean,
        multiple?: boolean,
        validateTogether?: string[],
        options?: OptionType[],
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
        mod,
        disabled,
        multiple,
        validateTogether,
        options,
        children,
    }: Props = $props();

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);
    let fieldValidation: ValidatedField;
    let opts: Options = {};

    $effect(() => {
        let valueArray: any[] = multiple ? value : [value];

        for (let optionKey in opts) {
            opts[optionKey].setSelected(valueArray.includes(optionKey));
        }
    })

    setContext<SelectFieldContext>('select', {
        registerField,
    });
    
    function registerField(name: string, setSelected: (isSelected: boolean) => void) {
        opts[name] = {
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

<div class="mb-4">
    <label for={id} class="form-label">{label}:</label>
    <select {id}
            {name}
            {size}
            class="form-select {mod}"
            class:is-invalid={!isValid}
            {oninput}
            {onfocus}
            {onfocusout}
            {disabled}
            {multiple}>
        {@render children()}
        {#if options}
            {#each options as o}
                <Option value={o.Value} selected={o.Value == value}>{o.Label}</Option>
            {/each}
        {/if}
    </select>
    <FieldValidation {name} {label} {value} shouldValidate={isVisited} bind:isValid bind:this={fieldValidation} {validateTogether} />
</div>