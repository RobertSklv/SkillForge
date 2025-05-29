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
        disabled?: boolean,
        onclick?: MouseEventHandler<HTMLButtonElement>,
        children: Snippet
    }

    let {
        color = 'primary',
        size,
        mod,
        isSubmitButton,
        disabled,
        onclick,
        children
    }: Props = $props();

    let isLoading = $state<boolean>(false);

    let formContext = getContext<FormContext>('form');

    async function onclickPrivate(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        isLoading = true;

        if (isSubmitButton) {
            await formContext?.submit();
        }

        await onclick?.(event);

        isLoading = false;
    }
</script>

<button type="button" class="btn btn-{color} {mod} {size ? 'btn-' + size : ''}" onclick={onclickPrivate} disabled={disabled || isLoading}>
    {#if isLoading}
        <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
        <span role="status">Processing...</span>
    {:else}
        {@render children()}
    {/if}
</button>