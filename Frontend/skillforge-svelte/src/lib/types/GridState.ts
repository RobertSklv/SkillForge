export default interface GridState {
    p?: number | null;
    q?: string | null,
    limit?: number | null;
    sortBy?: string | null;
    sortOrder?: 'asc' | 'desc' | string | null;
}