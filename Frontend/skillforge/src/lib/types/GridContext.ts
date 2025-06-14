export default interface GridContext {
    generateSearchUrl: (param: string, value: any) => string,
    updateState: (param: string, value: any) => Promise<void>,
}