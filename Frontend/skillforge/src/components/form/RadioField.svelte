<script lang="ts">
	import type FormContext from "$lib/types/FormContext";
	import { getContext, onMount } from "svelte";

    interface Props {
        id: string,
        name: string,
        label?: string,
        value: any,
        group: any,
        disabled?: boolean,
        isSwitch?: boolean,
        isInline?: boolean,
        isReverse?: boolean,
    }

    var {
        id,
        name = id,
        label = name,
        value,
        group = $bindable(),
        disabled,
        isSwitch,
        isInline,
        isReverse,
    }: Props = $props();

    const formContext = getContext<FormContext>('form');

    export function reset() {
        group = formContext?.getFieldDefaultValue(name);
    }

    onMount(() => {
        formContext?.registerField(name, reset);
    })
</script>

<div class="form-check"
     class:form-switch={isSwitch}
     class:form-check-inline={isInline}
     class:form-check-reverse={isReverse}>
    <input {id}
            {name}
            {value}
            bind:group
            type="radio"
            class="form-check-input"
            {disabled}>
    <label for={id} class="form-check-label">{label}</label>
</div>