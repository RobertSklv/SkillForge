<script lang="ts">
	import Form from '$components/form/Form.svelte';
	import TextAreaField from '$components/form/text-area-field/TextAreaField.svelte';
	import Modal from '$components/modal/Modal.svelte';
	import ModalHeader from '$components/modal-header/ModalHeader.svelte';
	import { addToast } from '$lib/stores/toastStore';
	import type ReportFormData from 'skillforge-common/types/ReportFormData';
	import { writable } from 'svelte/store';
	import RadioGroup from '$components/form/radio-group/RadioGroup.svelte';
	import RadioField from '$components/form/radio-field/RadioField.svelte';
	import Button from '$components/button/Button.svelte';
	import { reportFormOptionsStore } from '$lib/stores/reportFormOptionsStore';

	interface Props {
		entityId?: number;
		entityName?: string;
		action: string;
		show: boolean;
	}

	let {
		entityId,
		entityName,
		action,
		show = $bindable<boolean>()
	}: Props = $props();

	let formData = writable<ReportFormData>({
		Id: entityId,
		Name: entityName,
		Reason: 0
	});

	function onSuccess() {
		addToast('Report submitted successfully.', 'success');
		show = false;
	}
</script>

<Modal bind:show verticallyCentered>
	{#snippet header()}
		<ModalHeader title="Report article" />
	{/snippet}

	{#if $reportFormOptionsStore}
		<div class="px-4">
			<Form {action} formData={$formData} {onSuccess} resetMode="onsuccess">
				<RadioGroup name="Reason">
					{#each $reportFormOptionsStore.ViolationOptions as o}
						<RadioField
							id="Reason-{o.Value}"
							name="Reason"
							value={o.Value}
							label={o.Label}
							mod="mb-3"
							bind:group={$formData.Reason}
						/>
					{/each}
				</RadioGroup>

				{#if $formData.Reason === 8}
					<TextAreaField
						id="Message"
						rows="4"
						placeholder="Message..."
						bind:value={$formData.Message}
					/>
				{/if}

				<div class="text-center">
					<Button mod="mt-3" isSubmitButton>Submit</Button>
				</div>
			</Form>
		</div>
	{/if}
</Modal>
