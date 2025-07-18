import type ValidationRule from "./ValidationRule";

export default interface ValidationRules {
    [key: string]: ValidationRule[]
}