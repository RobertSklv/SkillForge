<script lang="ts">
	import type ModalContext from "skillforge-common/types/ModalContext";
	import { getContext } from "svelte";

    interface Props {
        title?: string
    }

    let {
        title
    }: Props = $props();

    let modalContext = getContext<ModalContext>('modal');
</script>

<div class="modal-header">
    <h2 class="modal-title fs-5">{title}</h2>
    <button type="button" class="btn-close" aria-label="Close" onclick={modalContext.close}></button>
</div>