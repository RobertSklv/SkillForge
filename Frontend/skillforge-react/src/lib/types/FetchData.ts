import type QueryParams from "./QueryParams";

export default interface FetchData {
    init?: RequestInit,
    query?: QueryParams,
    authToken?: string,
    contentTypeApplicationJson?: boolean
}