export default interface ArticleUpsertFormData {
    Id?: number,
    Image?: string | undefined,
    Title: string,
    Content: string,
    Tags: string[],
}