<script lang="ts">
	import Icon from '$components/icon/Icon.svelte';
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import { uploadImage } from 'skillforge-common/api/client';
	import type { ImageUploadType } from 'skillforge-common/types/ImageUploadType';
	import Button from '../../button/Button.svelte';
	import FileInputField from '../file-input-field/FileInputField.svelte';

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
    let hasFile = $derived<boolean>(!!url);
    let files = $state<FileList>();
    let fileInputValue = $state<any>();

    async function onchange() {
        let file: File | undefined = files?.[0];

        if (file) {
            isLoading = true;
            url = await uploadImage(file, imageUploadType);
            isLoading = false;
        }
    }

    function onRemoveClick() {
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
        <div class="m-4 d-flex justify-content-center">
            <div class="position-relative">
                <img src={PUBLIC_BACKEND_DOMAIN + url} alt={imageAlt} class="rounded preview-image" />
                <div class="d-flex justify-content-end position-absolute start-0 top-0 end-0">
                    <Button color="danger" onclick={onRemoveClick}>
                        <Icon type="trash-fill" />
                    </Button>
                </div>
            </div>
        </div>
    {/if}
</div>

<style>
    .preview-image {
        max-width: 250px;
    }
</style>