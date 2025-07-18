import type ArticleUpsertFormData from "./ArticleUpsertFormData";
import type Option from "./OptionType";

export default interface ArticleUpsertPageModel {
    CurrentState?: {
        Model: ArticleUpsertFormData,
        IsApproved: boolean,
    },
}