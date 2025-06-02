export default interface ArticleUpsertFormData {
    Id?: number,
    CategoryId: number,
    Image?: string | undefined,
    Title: string,
    Content: string,
    Tags: string[],
}