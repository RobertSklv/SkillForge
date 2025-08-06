export default interface SelectFieldContext {
    registerFieldValidation: (name: string, setSelected: (isSelected: boolean) => void) => void,
}