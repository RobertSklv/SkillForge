export default interface ErrorResponse {
    detail?: string,
    errors: {
        [key: string]: string[]
    },
    type: string,
    title: string,
    status: number,
    traceId: string
}