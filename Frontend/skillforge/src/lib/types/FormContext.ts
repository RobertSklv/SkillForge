import type ValidationRule from "./ValidationRule";

export default interface FormContext {
    registerField: (name: string, validate: (onlyVisited: boolean) => string[] | null, setErrors: (errors: string[]) => void) => void,
    updateField: (name: string, errors: string[]) => void,
    getValidationRules: (name: string) => ValidationRule[],
    validateFields: (fieldNames: string[]) => void,
    submit: () => Promise<void>,
}