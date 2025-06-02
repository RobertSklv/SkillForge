import type QueryParams from "./QueryParams";
import type SvelteFetch from "./SvelteFetch";

export default interface FetchData {
    fetchFunction?: SvelteFetch,
    init?: RequestInit,
    query?: QueryParams,
    contentTypeApplicationJson?: boolean
}