<script lang="ts">
	import { onDestroy, onMount } from 'svelte';
    import 'quill/dist/quill.snow.css';
	import { browser } from '$app/environment';
	import FieldValidation from './FieldValidation.svelte';
	import { uploadImage } from '$lib/api/client';
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import type { ImageUploadType } from '$lib/types/ImageUploadType';

    interface Props {
        id: string,
        label?: string,
        content: string,
        height?: number,
        imageUploadType: ImageUploadType
    }

    let {
        id,
        label = id,
        content = $bindable(),
        height = 300,
        imageUploadType,
    }: Props = $props();

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);

    $effect(() => {
        let convertedContent = quillEditor?.clipboard.convert({
            html: content
        });
        quillEditor?.setContents(convertedContent);
    });

    let editorWrapperElement: any;
    let quillEditor: any;

    function onTextChange() {
        content = quillEditor.getSemanticHTML();
    }

    function onSelectionChange() {
        isVisited = true;
    }

    onMount(async () => {
        const toolbarOptions = [
            ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
            ['blockquote', 'code-block'],
            ['link', 'image', 'formula'],

            [{ 'header': 1 }, { 'header': 2 }],               // custom button values
            [{ 'list': 'ordered'}, { 'list': 'bullet' }, { 'list': 'check' }],
            [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
            [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
            [{ 'direction': 'rtl' }],                         // text direction

            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

            [{ 'color': [] }],                                // dropdown with defaults from theme

            ['clean']                                         // remove formatting button
        ];

        // Quill must be dynamically imported because the module relies on the DOM and throws an error during SSR.
        let Quill: any = (await import('quill')).default;

        quillEditor = new Quill('#' + id, {
            modules: {
                toolbar: toolbarOptions
            },
            theme: 'snow',
        });

        let convertedContent = quillEditor.clipboard.convert({
            html: content
        });
        quillEditor.setContents(convertedContent);

        quillEditor.on('text-change', onTextChange);
        quillEditor.on('selection-change', onSelectionChange);

        quillEditor.getModule('toolbar').addHandler('image', () => {
            const input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', '.jpg, .jpeg, .png');
            input.click();

            input.onchange = async () => {
                const file = input.files?.[0];
                if (file) {
                    const url = PUBLIC_BACKEND_DOMAIN + (await uploadImage(file, imageUploadType));

                    const range = quillEditor.getSelection();
                    if (range) {
                        quillEditor.insertEmbed(range.index, 'image', url);
                    }
                }
            };
        });
    });

    onDestroy(() => {
        if (browser) {
            editorWrapperElement.innerHTML = `<div id="${id}" style="height: ${height}px"></div>`;
        }
    });
</script>

<div class="mb-4">
    <p class="form-label">{label}:</p>
    <div class="quill-editor-wrapper" class:is-invalid={!isValid} bind:this={editorWrapperElement}>
        <div {id} style="height: {height}px"></div>
    </div>
    <FieldValidation name={id} {label} value={content} shouldValidate={isVisited} bind:isValid />
</div>