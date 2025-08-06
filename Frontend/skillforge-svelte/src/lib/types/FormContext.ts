import type ValidationRule from "./ValidationRule";

export default interface FormContext {
    registerField: (name: string, reset: () => void) => void,
    getFieldDefaultValue: (name: string) => any,
    registerFieldValidation: (name: string, validate: (onlyVisited: boolean) => string[] | null, setErrors: (errors: string[]) => void) => void,
    updateFieldValidation: (name: string, errors: string[]) => void,
    getValidationRules: (name: string) => ValidationRule[],
    validateFields: (fieldNames: string[]) => void,
    submit: () => Promise<void>,
}