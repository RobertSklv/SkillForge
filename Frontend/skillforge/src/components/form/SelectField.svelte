<script lang="ts">
	import type ValidatedField from '$lib/types/ValidatedField';
	import { getContext, onMount, setContext, type Snippet } from 'svelte';
	import FieldValidation from './FieldValidation.svelte';
	import type SelectFieldContext from '$lib/types/SelectFieldContext';
    import type OptionType from '$lib/types/OptionType';
	import type FormContext from '$lib/types/FormContext';
	import Select from './Select.svelte';

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
        children?: Snippet,
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

    const formContext = getContext<FormContext>('form');

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);
    let fieldValidation: ValidatedField;
    let opts: Options = {};

    $effect(() => {
        let valueArray: any[] = multiple ? value : [value];

        valueArray = valueArray.map(v => v?.toString());

        for (let optionKey in opts) {
            opts[optionKey].setSelected(valueArray.includes(optionKey));
        }
    })

    setContext<SelectFieldContext>('select', {
        registerFieldValidation,
    });
    
    function registerFieldValidation(name: string, setSelected: (isSelected: boolean) => void) {
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

    export function reset() {
        value = formContext?.getFieldDefaultValue(name);
    }

    onMount(() => {
        formContext?.registerField(name, reset);
    })
</script>

<div class="mb-4">
    <label for={id} class="form-label">{label}:</label>
    <Select {id}
            {name}
            {size}
            mod="{mod} {!isValid ? 'is-invalid' : ''}"
            {oninput}
            {onfocus}
            {onfocusout}
            {disabled}
            {multiple}
            bind:value>
        {@render children?.()}
    </Select>
    <FieldValidation {name} {label} {value} shouldValidate={isVisited} bind:isValid bind:this={fieldValidation} {validateTogether} />
</div>