export default interface ValidationRule {
    validate: (value: any) => boolean,
    message: (label: string) => string,
}