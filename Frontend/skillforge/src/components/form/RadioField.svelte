<script lang="ts">
    interface Props {
        id: string,
        name: string,
        label?: string,
        value: any,
        group: any,
        disabled?: boolean,
        isSwitch?: boolean,
        isInline?: boolean,
        isReverse?: boolean,
    }

    var {
        id,
        name = id,
        label = name,
        value,
        group = $bindable(),
        disabled,
        isSwitch,
        isInline,
        isReverse,
    }: Props = $props();
</script>

<div class="form-check"
     class:form-switch={isSwitch}
     class:form-check-inline={isInline}
     class:form-check-reverse={isReverse}>
    <input {id}
            {name}
            {value}
            bind:group
            type="radio"
            class="form-check-input"
            {disabled}>
    <label for={id} class="form-check-label">{label}</label>
</div>