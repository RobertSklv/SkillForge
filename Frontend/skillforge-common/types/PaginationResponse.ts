export default interface PaginationResponse<T> {
    Items: T[];
    ItemCount: number;
    TotalItems: number;
}