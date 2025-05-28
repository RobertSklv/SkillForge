<script lang="ts">
	import { onDestroy, onMount } from 'svelte';
    import 'quill/dist/quill.snow.css';
	import { browser } from '$app/environment';

    interface Props {
        id: string,
        content: string,
    }

    let {
        id,
        content = $bindable()
    }: Props = $props();

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

    onMount(async () => {
        // Quill must be dynamically imported because the module relies on the DOM and throws an error during SSR.
        let Quill: any = (await import('quill')).default;

        quillEditor = new Quill('#' + id, {
            theme: 'snow',
        });

        let convertedContent = quillEditor.clipboard.convert({
            html: content
        });
        quillEditor.setContents(convertedContent);

        quillEditor.on('text-change', onTextChange);
    });

    onDestroy(() => {
        if (browser) {
            editorWrapperElement.innerHTML = `<div id="${id}"></div>`;
        }
    });
</script>

<div class="quill-editor-wrapper mb-4" bind:this={editorWrapperElement}>
    <div {id}></div>
</div>