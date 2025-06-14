import type ArticleUpsertFormData from "./ArticleUpsertFormData";
import type Option from "./OptionType";

export default interface ArticleUpsertPageModel {
    CategoryOptions: Option[],
    CurrentState?: {
        Model: ArticleUpsertFormData,
        IsApproved: boolean,
    },
}