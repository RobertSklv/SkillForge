import type ArticleUpsertFormData from "./ArticleUpsertFormData";
import type Option from "./Option";

export default interface ArticleUpsertPageModel {
    CategoryOptions: Option[],
    CurrentState?: {
        Model: ArticleUpsertFormData,
        IsApproved: boolean,
    },
}