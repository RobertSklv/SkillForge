import { AuthorBox } from '@components/author-box/AuthorBox';
import { Block } from '@components/block/Block';
import { CommentLimited } from '@components/comment-limited/CommentLimited';
import { Icon } from '@components/icon/Icon';
import { Link } from '@components/link/Link';
import { RateButtons } from '@components/rate-buttons/RateButtons';
import { TagLink } from '@components/tag-link/TagLink';
import type ArticleCardType from 'skillforge-common/types/ArticleCardType';
import { getImagePath } from 'skillforge-common/util';

export interface IArticleCardProps {
    data: ArticleCardType;
    showComments?: boolean;
    classes?: string;
}

export function ArticleCard({ data, showComments = true, classes }: IArticleCardProps) {
    return (
        <Block classes={classes}>
            {data.CoverImage &&
                <img
                    src={getImagePath(data.CoverImage)}
                    className="card-img-top rounded-top-3 object-fit-cover"
                    alt="Article cover"
                    style={{
                        height: '250px',
                        maxWidth: '100%'
                    }}
                />
            }
            <AuthorBox name={data.Author.Name} avatarImage={data.Author.AvatarImage} date={data.DatePublished} classes="mt-3" />
            <div className="card-body mx-5">
                <div className="d-flex align-items-start">
                    <h2 className="card-title fw-bold">
                        <a href="/article/{data.ArticleId}" className="text-decoration-none">{data.Title}</a>
                    </h2>
                </div>
                <div className="row justify-content-between">
                    <div className="col-12 col-md-6">
                        <div className="tags d-flex flex-wrap flex-md-nowrap gap-1 justify-content-center justify-content-md-start">
                            {data.Tags.map(tag => {
                                return <TagLink size="small" background="fill" muted={true} data={tag} />;
                            })}
                        </div>
                    </div>
                    <div className="col-12 col-md-6">
                        <div className="d-flex justify-content-center justify-content-md-end align-items-center">
                            <RateButtons
                                data={data.RatingData}
                                subjectId={data.ArticleId}
                                size="small"
                                type="article"
                                readonly={true}
                            />
                            {data.Comments.length &&
                                <div className="text-primary ms-3">
                                    <Icon type="chat" classes="fs-6" />
                                    <small>{data.TotalComments}</small>
                                </div>
                            }
                        </div>
                    </div>
                </div>
            </div>
            {showComments && (
                <>
                    <div>
                        {data.Comments.map(comment => {
                            return <CommentLimited data={comment} />;
                        })}
                    </div>
                    {(data.Comments.length < data.TotalComments) &&
                        <div className="text-center mt-2 mb-4">
                            <Link href={`/article/${data.ArticleId}`} background="fill">
                                View all comments
                            </Link>
                        </div>
                    }
                </>
            )}
        </Block>
    );
}
