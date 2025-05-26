<script lang="ts">
	import type { BootstrapColor } from "$lib/types/BootstrapColor";
	import type { BootstrapSize } from "$lib/types/BootstrapSize";
	import type FormContext from "$lib/types/FormContext";
	import { getContext, type Snippet } from "svelte";
	import type { MouseEventHandler } from "svelte/elements";

    interface Props {
        color?: BootstrapColor,
        size?: BootstrapSize,
        mod?: string,
        isSubmitButton?: boolean,
        onclick?: MouseEventHandler<HTMLButtonElement>,
        children: Snippet
    }

    let {
        color = 'primary',
        size,
        mod,
        isSubmitButton,
        onclick,
        children
    }: Props = $props();

    let formContext = getContext<FormContext>('form');

    function onclickPrivate(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        if (isSubmitButton) {
            formContext?.submit();
        }

        onclick?.(event);
    }
</script>

<button type="button" class="btn btn-{color} {mod} {size ? 'btn-' + size : ''}" onclick={onclickPrivate}>
    {@render children()}
</button>