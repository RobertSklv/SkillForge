<script lang="ts">
	import type FormContext from "$lib/types/FormContext";
    import type ErrorResponse from "$lib/types/ErrorResponse";
	import type ValidationRule from "$lib/types/ValidationRule";
	import type ValidationRules from "$lib/types/ValidationRules";
	import { setContext, type Snippet } from "svelte";
	import { requestApi } from "$lib/api/client";
	import { addToast } from "$lib/stores/toastStore";

    interface Props {
        action: string,
        method?: 'GET' | 'POST' | 'DIALOG',
        formData: any,
        isMultipartFormData?: boolean,
        onSuccess: (response: any) => void,
        children: Snippet,
        validationRules?: ValidationRules,
        resetMode?: 'never' | 'onsuccess' | 'onsubmit',
    }

    interface FieldValidations {
        [key: string]: {
            validate: (onlyVisited: boolean) => string[] | null,
            setErrors: (errors: string[]) => void,
            errors: string[]
        }
    }

    interface Fields {
        [key: string]: {
            reset: () => void
        }
    }

    const {
        action,
        method = 'POST',
        formData,
        isMultipartFormData = false,
        onSuccess,
        children,
        validationRules,
        resetMode = 'never',
    }: Props = $props();

    let fields: Fields = {};
    let fieldValidations: FieldValidations = {};
    let initialFormData = JSON.parse(JSON.stringify(formData ?? '{}'));

    function registerField(name: string, reset: () => void) {
        fields[name] = {
            reset
        };
    }

    function getFieldDefaultValue(name: string): any {
        return initialFormData[name];
    }

    function registerFieldValidation(
            name: string,
            validate: (onlyVisited: boolean) => string[] | null,
            setErrors: (errors: string[]) => void): void {

        fieldValidations[name] = {
            validate,
            setErrors,
            errors: []
        }
    }

    function updateFieldValidation(name: string, errors: string[]): void {
        fieldValidations[name].errors = errors;
    }

    function getValidationRules(name: string): ValidationRule[] {
        if (typeof validationRules === 'undefined' || !(name in validationRules)) {
            return [];
        }

        return validationRules[name];
    }

    setContext<FormContext>('form', {
        registerField,
        getFieldDefaultValue,
        registerFieldValidation,
        updateFieldValidation,
        getValidationRules,
        validateFields,
        submit,
    });

    export function validate(): boolean {
        let isValid = true;

        for (let fieldName in fieldValidations) {
            let fieldErrors = fieldValidations[fieldName].validate(false);

            if (fieldErrors == null) {
                // should never happen because onlyVisited argument is false in the validate function call.
                continue;
            }

            if (isValid && !!fieldErrors.length) {
                isValid = false;
            }
        }

        return isValid;
    }

    export function validateFields(fieldNames: string[]) {
        for (let fieldName of fieldNames) {
            fieldValidations[fieldName].validate(true);
        }
    }

    export async function submit() {
        if (!validate()) {
            return;
        }

        let init: RequestInit = {
            method,
            credentials: 'include',
            headers: {
                'Accept': 'application/json',
            }
        };

        if (!isMultipartFormData) {
            init.headers = {
                ...init.headers,
                'Content-Type': 'application/json',
            };

            init.body = JSON.stringify(formData);
        } else {
            let fd = new FormData();
            for (let fieldName in formData) {
                fd.append(fieldName, formData[fieldName] as string | Blob);
            }

            init.body = fd;
        }

        return requestApi(action, {
            init
        })
            .then(r => {
                onSuccess(r);

                if (resetMode === 'onsuccess') {
                    reset();
                }
            })
            .catch((e: ErrorResponse) => {
                for (let fieldName in e.errors) {
                    let errors = e.errors[fieldName];

                    if (fieldValidations[fieldName]) {
                        fieldValidations[fieldName].errors = errors;
                        fieldValidations[fieldName].setErrors(errors);
                    }
                }

                if (e.detail || e.title) {
                    console.error(e.detail ?? e.title);
                    addToast(e.detail ?? e.title, 'danger');
                }
            })
            .finally(() => {
                if (resetMode === 'onsubmit') {
                    reset();
                }
            });
    }

    export function reset() {
        for (let fieldName in fields) {
            fields[fieldName].reset();
        }
    }
</script>

<form {action} {method} enctype={isMultipartFormData ? 'multipart/form-data' : undefined}>
    {@render children()}
</form>