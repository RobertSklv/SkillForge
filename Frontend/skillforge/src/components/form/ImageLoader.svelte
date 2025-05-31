<script lang="ts">
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import { uploadImage } from '$lib/api/client';
	import type { ImageUploadType } from '$lib/types/ImageUploadType';
	import Button from '../button/Button.svelte';
	import FileInputField from './FileInputField.svelte';

	interface Props {
		id: string;
		name?: string;
		label?: string;
        imageAlt: string,
        url: string | undefined,
        imageUploadType: ImageUploadType
	}

    let {
        id,
        name = id,
        label = name,
        imageAlt,
        url = $bindable(),
        imageUploadType,
    }: Props = $props();

    let isLoading = $state<boolean>(false);
    let filename = $state<string | undefined>();
    let hasFile = $derived<boolean>(!!filename);
    let files = $state<FileList>();
    let fileInputValue = $state<any>();

    async function onchange() {
        let file: File | undefined = files?.[0];

        if (file) {
            isLoading = true;
            url = await uploadImage(file, imageUploadType);
            isLoading = false;

            filename = PUBLIC_BACKEND_DOMAIN + url;
        }
    }

    function onRemoveClick() {
        filename = undefined;
        url = undefined;
        fileInputValue = '';
    }
</script>

<div class="mb-4">
    <FileInputField
        {id}
        {name}
        {label}
        accept=".jpg, .jpeg, .png"
        bind:files
        bind:value={fileInputValue}
        {onchange}
    />
    {#if isLoading}
        <div class="position-relative m-4">
            <div class="d-flex justify-content-center align-items-center" style="height: 150px">
                <strong role="status">Loading...</strong>
                <div class="spinner-border ms-3" aria-hidden="true"></div>
            </div>
        </div>
    {:else if hasFile}
        <div class="position-relative m-4">
            <img src={filename} alt={imageAlt} class="w-100 rounded" />
            <div class="d-flex justify-content-end position-absolute start-0 top-0 end-0">
                <Button color="danger" onclick={onRemoveClick}>
                    <i class="bi bi-trash-fill"></i>
                </Button>
            </div>
        </div>
    {/if}
</div>
