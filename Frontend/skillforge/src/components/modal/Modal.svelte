<script lang="ts">
	import Scrollable from '$components/scrollable/Scrollable.svelte';
	import { type Snippet } from 'svelte';
	import { fade } from 'svelte/transition';
    import './modal.scss';
	import { clickOutside } from '$lib/util';

	interface Props {
		title?: string;
		children?: Snippet;
		footer?: Snippet;
        show: boolean,
		size?: 'default' | 'sm' | 'lg' | 'xl';
		verticallyCentered?: boolean;
		scrollable?: boolean;
	}

	let {
		title,
		children,
		footer,
        show = $bindable<boolean>(false),
		size = 'default',
		verticallyCentered,
		scrollable
	}: Props = $props();

    function close() {
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
                <div class="modal-header">
                    <h2 class="modal-title fs-5">{title}</h2>
                    <button type="button" class="btn-close" aria-label="Close" onclick={close}></button>
                </div>
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