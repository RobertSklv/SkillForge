<script lang="ts">
	import type SelectFieldContext from "skillforge-common/types/SelectFieldContext";
	import { getContext, onMount, type Snippet } from "svelte";

    interface Props {
        value: string,
        label?: string,
        selected?: boolean,
        disabled?: boolean,
        children?: Snippet,
    }

    let {
        value,
        label,
        selected,
        disabled,
        children,
    }: Props = $props();

    const selectContext = getContext<SelectFieldContext>('select');

    function setSelected(isSelected: boolean) {
        selected = isSelected;
    }

    onMount(() => {
        selectContext.registerFieldValidation(value, setSelected);
    });
</script>

<option {value} {label} {selected} {disabled}>
    {@render children?.()}
</option>