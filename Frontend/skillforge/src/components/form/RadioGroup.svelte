<script lang="ts">
	import type Option from "$lib/types/OptionType";
	import type { Snippet } from "svelte";
	import RadioField from "./RadioField.svelte";
	import FieldValidation from "./FieldValidation.svelte";
	import type ValidatedField from "$lib/types/ValidatedField";
    import './radio-group.scss';

    interface Props {
        name: string,
        label?: string,
        options?: Option[],
        group?: any,
        isSwitch?: boolean,
        isInline?: boolean,
        isReverse?: boolean,
        validateTogether?: string[],
        children?: Snippet,
    }

    let {
        name,
        label,
        options,
        group = $bindable(),
        isSwitch,
        isInline,
        isReverse,
        validateTogether,
        children,
    }: Props = $props();

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);
    let fieldValidation: ValidatedField;
    let lastValue: any | null = null;

    $effect(() => {
        if (group !== lastValue) {
            fieldValidation.validate();

            lastValue = group;
            isVisited = true;
        }
    });
</script>

<div class="mb-4">
    <div role="radiogroup" class="radio-group" class:is-invalid={!isValid}>
        {#if label}
            <strong class="d-inline-block mb-2">{label}:</strong>
        {/if}
        {#if options}
            {#each options as { Value, Label }}
                <RadioField id="{name}-{Value}" {name} label={Label} value={Value} {isSwitch} {isInline} {isReverse} bind:group />
            {/each}
        {/if}
        {@render children?.()}
    </div>
    <FieldValidation {name}
        label={label ?? name}
        value={group}
        shouldValidate={isVisited}
        bind:isValid
        bind:this={fieldValidation}
        {validateTogether}
    />
</div>