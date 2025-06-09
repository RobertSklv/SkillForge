<script lang="ts">
	import Scrollable from '$components/scrollable/Scrollable.svelte';
	import { setContext, type Snippet } from 'svelte';
	import { fade } from 'svelte/transition';
    import './modal.scss';
	import { clickOutside } from '$lib/util';
	import type ModalContext from '$lib/types/ModalContext';

	interface Props {
		children?: Snippet;
        header?: Snippet,
		footer?: Snippet;
        show: boolean,
		size?: 'default' | 'sm' | 'lg' | 'xl';
		verticallyCentered?: boolean;
		scrollable?: boolean;
	}

	let {
		children,
        header,
		footer,
        show = $bindable<boolean>(false),
		size = 'default',
		verticallyCentered,
		scrollable
	}: Props = $props();

    setContext<ModalContext>('modal', {
        close
    });

    export function close() {
        show = false;
    }
</script>

{#if show}
    <div
        class="modal fade show d-block"
        tabindex="-1"
        aria-modal={show ? 'true' : undefined}
        role={show ? 'dialog' : undefined}
        transition:fade={{ duration: 100 }}
    >
        <div
            class="modal-dialog modal-fullscreen-sm-down {size === 'default' ? '' : `modal-${size}`}"
            class:modal-dialog-centered={verticallyCentered}
            class:modal-dialog-scrollable={scrollable}
            use:clickOutside={close}
        >
            <div class="modal-content">
                {@render header?.()}
                {#if children}
                    {#if scrollable}
                        <Scrollable class="modal-body">
                            {@render children()}
                        </Scrollable>
                    {:else}
                        <div class="modal-body">
                            {@render children()}
                        </div>
                    {/if}
                {/if}
                {#if footer}
                    <div class="modal-footer">
                        {@render footer()}
                    </div>
                {/if}
            </div>
        </div>
    </div>
    <div class="modal-backdrop show"></div>
{/if}