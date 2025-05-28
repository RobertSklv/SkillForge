<script lang="ts">
    interface Props {
        id: string,
        name?: string,
        label?: string,
        files: FileList | null | undefined,
        accept?: string,
        mod?: string,
        disabled?: boolean,
    }

    var {
        id,
        name = id,
        label = name,
        files = $bindable(),
        accept,
        mod,
        disabled,
    }: Props = $props();

    let selectedFiles: FileList;

    // Called when the file input changes
    function handleFileChange(event: Event) {
        const target = event.target as HTMLInputElement;

        if (target.files && target.files.length > 0) {
            selectedFiles = target.files;
            console.log("Selected files:", selectedFiles);
        }
    }
</script>

<div class="mb-4">
    <label for={id} class="form-label">{label}:</label>
    <input class="form-control {mod}" type="file" {id} {name} {accept} bind:files {disabled}>
</div>