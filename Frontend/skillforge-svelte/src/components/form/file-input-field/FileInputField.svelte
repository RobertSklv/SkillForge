<script lang="ts">
	import type FormContext from "skillforge-common/types/FormContext";
	import { getContext, onMount } from "svelte";
	import type { ChangeEventHandler } from "svelte/elements";

    interface Props {
        id: string,
        name?: string,
        label?: string,
        files?: FileList | null | undefined,
        value?: any,
        accept?: string,
        mod?: string,
        disabled?: boolean,
        onchange?: ChangeEventHandler<HTMLInputElement>,
    }

    var {
        id,
        name = id,
        label = name,
        files = $bindable(),
        value = $bindable(),
        accept,
        mod,
        disabled,
        onchange,
    }: Props = $props();

    const formContext = getContext<FormContext>('form');

    export function reset() {
        value = formContext?.getFieldDefaultValue(name);
    }

    onMount(() => {
        formContext?.registerField(name, reset);
    })
</script>

<div class="mb-4">
    <label for={id} class="form-label">{label}:</label>
    <input class="form-control {mod}" type="file" {id} {name} {accept} bind:files bind:value {onchange} {disabled}>
</div>