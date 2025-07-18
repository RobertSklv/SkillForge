<script lang="ts">
	import type ValidatedField from '$lib/types/ValidatedField';
	import { type Snippet } from 'svelte';
	import FieldValidation from '../field-validation/FieldValidation.svelte';
    import type OptionType from '$lib/types/OptionType';
	import Select from '../select/Select.svelte';

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

<div class="mb-4">
    <label for={id} class="form-label">{label}:</label>
    <Select {id}
            {name}
            {size}
            mod="{mod} {!isValid ? 'is-invalid' : ''}"
            {options}
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