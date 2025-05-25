<script lang="ts">
	import type { BootstrapColor } from "$lib/types/BootstrapColor";
	import type { BootstrapSize } from "$lib/types/BootstrapSize";
	import type { Snippet } from "svelte";
	import type { MouseEventHandler } from "svelte/elements";

    interface Props {
        color?: BootstrapColor,
        size?: BootstrapSize,
        mod?: string,
        onclick?: MouseEventHandler<HTMLButtonElement>,
        children: Snippet
    }

    let {
        color = 'primary',
        size,
        mod,
        onclick,
        children
    }: Props = $props();

</script>

<button type="button" class="btn btn-{color} {mod} {size ? 'btn-' + size : ''}" {onclick}>
    {@render children()}
</button>