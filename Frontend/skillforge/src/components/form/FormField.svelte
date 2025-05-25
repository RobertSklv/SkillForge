<script lang="ts" generics="T">
	import type { InputType } from '$lib/types/InputType';

    interface Props {
        id?: string,
        name?: string,
        label: string,
        type?: InputType,
        value: T,
        placeholder?: string,
        disabled?: boolean,
    }

    const {
        id,
        name = id,
        label,
        type = 'text',
        value = $bindable(),
        placeholder,
        disabled,
    }: Props = $props();
</script>

<div class="mb-3">
  <label for={id} class="form-label">{label}:</label>
  <input {id} {type} {name} {placeholder} class="form-control" {disabled}>
</div>