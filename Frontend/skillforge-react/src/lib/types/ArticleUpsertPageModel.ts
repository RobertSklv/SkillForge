import type ArticleUpsertFormData from "./ArticleUpsertFormData";

export default interface ArticleUpsertPageModel {
    CurrentState?: {
        Model: ArticleUpsertFormData,
        IsApproved: boolean,
    },
}