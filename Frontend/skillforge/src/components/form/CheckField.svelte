<script lang="ts">
	import FieldValidation from "./FieldValidation.svelte";

    interface Props {
        id: string,
        name?: string,
        label?: string,
        checked: boolean,
        disabled?: boolean,
        isSwitch?: boolean,
        isInline?: boolean,
        isReverse?: boolean,
        validateTogether?: string[],
    }

    var {
        id,
        name = id,
        label = name,
        checked = $bindable(),
        disabled,
        isSwitch,
        isInline,
        isReverse,
        validateTogether,
    }: Props = $props();

    let isValid = $state<boolean>(true);
    let isVisited = $state<boolean>(false);

    function onfocus() {
        isVisited = true;
    }
</script>

<div class="form-check"
     class:form-switch={isSwitch}
     class:form-check-inline={isInline}
     class:form-check-reverse={isReverse}>
    <input {id}
            {name}
            bind:checked
            type="checkbox"
            class="form-check-input"
            class:is-invalid={!isValid}
            {onfocus}
            {disabled}>
    <label for={id} class="form-check-label">{label}</label>
    <FieldValidation {name} {label} value={checked} shouldValidate={isVisited} {isValid} {validateTogether} />
</div>