export default interface ValidationRule {
    validate: (value: any) => boolean | Promise<boolean>,
    message: (label: string) => string,
}