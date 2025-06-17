<script lang="ts">
	import { fade } from 'svelte/transition';
	import { flip } from 'svelte/animate';
	import { removeToast, toastStore } from '$lib/stores/toastStore';
	import Icon from '$components/icon/Icon.svelte';

	const iconMap = {
		success: 'check-circle-fill',
		warning: 'exclamation-triangle-fill',
		danger: 'x-circle-fill',
		info: 'info-circle-fill'
	};
</script>

<div
	class="position-fixed pe-none start-0 bottom-0 end-0 p-3 z-3 d-flex flex-column-reverse align-items-end gap-2"
>
	{#each $toastStore.toasts as toast (toast.uid)}
		<div
			class="alert alert-{toast.type} alert-dismissible d-flex align-items-center w-auto"
			role="alert"
			transition:fade={{ duration: 200 }}
			animate:flip={{ duration: 200 }}
		>
			<Icon type={iconMap[toast.type]} mod="me-2" />
			<div>{toast.message}</div>
			<button
				type="button"
				class="btn-close"
				aria-label="Close"
				onclick={() => removeToast(toast.uid)}
			></button>
		</div>
	{/each}
</div>
