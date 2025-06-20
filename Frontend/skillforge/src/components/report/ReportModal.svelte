<script lang="ts">
	import Form from '$components/form/Form.svelte';
	import TextAreaField from '$components/form/TextAreaField.svelte';
	import Modal from '$components/modal/Modal.svelte';
	import ModalHeader from '$components/modal/ModalHeader.svelte';
	import { addToast } from '$lib/stores/toastStore';
	import type ReportFormOptions from '$lib/types/ReportFormOptions';
	import type ReportFormData from '$lib/types/ReportFormData';
	import { writable } from 'svelte/store';
	import RadioGroup from '$components/form/RadioGroup.svelte';
	import RadioField from '$components/form/RadioField.svelte';
	import Button from '$components/button/Button.svelte';
	import { onMount } from 'svelte';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import { getReportFormOptions } from '$lib/api/client';

	interface Props {
		entityId: number;
		action: string;
		show: boolean;
	}

	let { entityId, action, show = $bindable<boolean>() }: Props = $props();

	let formOptions = $state<ReportFormOptions>();

	let formData = writable<ReportFormData>({
		Id: entityId,
		Reason: 0
	});

	function onSuccess() {
		addToast('Report submitted successfully.', 'success');
		show = false;
	}

	onMount(async () => {
		if ($currentUserStore) {
			formOptions = await getReportFormOptions();
		}
	});
</script>

<Modal bind:show verticallyCentered>
	{#snippet header()}
		<ModalHeader title="Report article" />
	{/snippet}

	{#if formOptions}
		<div class="px-4">
			<Form {action} formData={$formData} {onSuccess} resetMode="onsuccess">
				<RadioGroup name="Reason">
					{#each formOptions.ViolationOptions as o}
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
