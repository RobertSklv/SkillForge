import type ValidationRule from "./ValidationRule";

export default interface FormContext {
    registerField: (name: string, validate: () => string[], setErrors: (errors: string[]) => void) => void,
    updateField: (name: string, errors: string[]) => void,
    getValidationRules: (name: string) => ValidationRule[]
}