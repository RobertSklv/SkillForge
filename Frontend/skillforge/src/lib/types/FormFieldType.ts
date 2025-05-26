export default interface FormFieldType {
    validate: (onlyVisited: boolean) => string[] | null
}