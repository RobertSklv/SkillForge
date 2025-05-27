export default interface SelectFieldContext {
    registerField: (name: string, setSelected: (isSelected: boolean) => void) => void,
}