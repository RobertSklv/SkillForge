<script lang="ts">
	import { getContext, onMount, setContext, type Snippet } from 'svelte';
	import type SelectFieldContext from '$lib/types/SelectFieldContext';
    import type OptionType from '$lib/types/OptionType';
	import Option from './Option.svelte';
	import type FormContext from '$lib/types/FormContext';

    interface Props {
        id: string,
        name?: string,
        value: any,
        size?: any,
        mod?: string,
        disabled?: boolean,
        multiple?: boolean,
        options?: OptionType[],
        children?: Snippet,
        onValueChange?: (value: any) => void,
        oninput?: (e: Event & { currentTarget: EventTarget & HTMLSelectElement; }) => void,
        onfocus?: (e: FocusEvent & { currentTarget: EventTarget & HTMLSelectElement; }) => void,
        onfocusout?: (e: FocusEvent & { currentTarget: EventTarget & HTMLSelectElement; }) => void,
    }

    interface Options {
        [key: string]: {
            setSelected: (isSelected: boolean) => void,
        }
    }

    var {
        id,
        name = id,
        value = $bindable(),
        size,
        mod,
        disabled,
        multiple,
        options,
        children,
        onValueChange,
        oninput,
        onfocus,
        onfocusout,
    }: Props = $props();

    const formContext = getContext<FormContext>('form');

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

    function oninputPrivate(e: Event & { currentTarget: EventTarget & HTMLSelectElement; }) {
        if (e.target != null) {
            if (multiple) {
                value = Array.from((e.target as HTMLSelectElement).selectedOptions).map(option => option.value);
            } else {
                value = (e.target as HTMLSelectElement).value;
            }
        }

        onValueChange?.(value);
        oninput?.(e);
    }

    export function reset() {
        value = formContext?.getFieldDefaultValue(name);
    }

    onMount(() => {
        formContext?.registerField(name, reset);
    })
</script>

<select {id}
        {name}
        {size}
        class="form-select {mod}"
        oninput={oninputPrivate}
        {onfocus}
        {onfocusout}
        {disabled}
        {multiple}>
    {@render children?.()}
    {#if options}
        {#each options as o}
            <Option value={o.Value}>{o.Label}</Option>
        {/each}
    {/if}
</select>