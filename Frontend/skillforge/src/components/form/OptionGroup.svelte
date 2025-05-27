<script lang="ts">
	import type { Snippet } from "svelte";

    interface Props {
        label: string,
        disabled?: boolean,
        children: Snippet,
    }

    let {
        label,
        disabled,
        children,
    }: Props = $props();
</script>

<optgroup {label} {disabled}>
    {@render children()}
</optgroup>