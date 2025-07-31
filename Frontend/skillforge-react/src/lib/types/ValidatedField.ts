export default interface ValidatedField {
    validate: (onlyVisited?: boolean) => string[] | null
}