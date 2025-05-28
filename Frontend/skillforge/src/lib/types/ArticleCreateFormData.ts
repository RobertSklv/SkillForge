export default interface ArticleCreateFormData {
    CategoryId: number,
    Image?: FileList | null,
    Title: string,
    Content: string
}