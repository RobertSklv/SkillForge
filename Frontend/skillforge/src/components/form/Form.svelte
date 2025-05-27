<script lang="ts" generics="T">
	import type FormContext from "$lib/types/FormContext";
    import type ErrorResponse from "$lib/types/ErrorResponse";
	import type ValidationRule from "$lib/types/ValidationRule";
	import type ValidationRules from "$lib/types/ValidationRules";
	import { setContext, type Snippet } from "svelte";
	import { fetchApi } from "$lib/api/client";

    interface Props {
        action: string,
        method?: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE',
        formData: T,
        onSuccess: (response: any) => void,
        children: Snippet,
        validationRules?: ValidationRules
    }

    interface Fields {
        [key: string]: {
            validate: (onlyVisited: boolean) => string[] | null,
            setErrors: (errors: string[]) => void,
            errors: string[]
        }
    }

    const {
        action,
        method = 'GET',
        formData,
        onSuccess,
        children,
        validationRules
    }: Props = $props();

    let fields: Fields = {};

    function registerField(
            name: string,
            validate: (onlyVisited: boolean) => string[] | null,
            setErrors: (errors: string[]) => void): void {

        fields[name] = {
            validate,
            setErrors,
            errors: []
        }
    }

    function updateField(name: string, errors: string[]): void {
        fields[name].errors = errors;
    }

    function getValidationRules(name: string): ValidationRule[] {
        if (typeof validationRules === 'undefined' || !(name in validationRules)) {
            return [];
        }

        return validationRules[name];
    }

    setContext<FormContext>('form', {
        registerField,
        updateField,
        getValidationRules,
        validateFields,
        submit,
    });

    export function validate(): boolean {
        let isValid = true;

        for (let fieldName in fields) {
            let fieldErrors = fields[fieldName].validate(false);

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
            fields[fieldName].validate(true);
        }
    }

    export function submit() {
        if (!validate()) {
            return;
        }

        let init: RequestInit = {
            method,
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        };

        if (method === 'POST' || method === 'PUT' || method === 'PATCH') {
            init.body = JSON.stringify(formData);
        }

        fetchApi(action, init)
            .then(r => {
                console.log(r);
                onSuccess(r);
            })
            .catch((e: ErrorResponse) => {
                for (let fieldName in e.errors) {
                    let errors = e.errors[fieldName];
                    fields[fieldName].errors = errors;
                    fields[fieldName].setErrors(errors);
                }

                if (e.detail) {
                    console.error(e.detail);
                }
            })
    }
</script>

<form>
    {@render children()}
</form>