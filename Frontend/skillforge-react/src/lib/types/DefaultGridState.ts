export default interface DefaultGridState {
    p: number;
    q?: string | undefined,
    limit: number;
    sortBy: string;
    sortOrder: 'asc' | 'desc' | string;
}