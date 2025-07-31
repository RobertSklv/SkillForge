import type CurrentUserRatingData from "./CurrentUserRatingData";

export default interface RatingData {
    ThumbsUp: number,
    ThumbsDown: number,
    UserRating: -1 | 0 | 1,
}