import { Icon } from '@components/icon/Icon';
import moment from 'moment';
import type TopArticleItemType from 'skillforge-common/types/TopArticleItemType';
import { formatRelativeTime } from 'skillforge-common/util';

export interface ITopArticleLinkProps {
    data: TopArticleItemType;
}

export function TopArticleLink ({ data }: ITopArticleLinkProps) {
  return (
    <a
        href={`/article/${data.ArticleId}`}
        className="list-group-item list-group-item-action"
        aria-label={`Article '${data.Title}' link`}
    >
        <div className="d-flex w-100 justify-content-between">
            <h4 className="h5 mb-1">{data.Title}</h4>
        </div>
        <small className="text-body-tertiary opacity-75">
            <Icon type="eye-fill" />
            <span>{data.ViewCount}</span>
            &nbsp;&nbsp;|&nbsp;&nbsp;
            <Icon type="chat-fill" />
            <span>{data.CommentCount}</span>
            &nbsp;&nbsp;|&nbsp;&nbsp;
            <time dateTime={moment(data.DatePublished).format('YYYY-MM-DD HH:mm')}>
                {formatRelativeTime(data.DatePublished)}
            </time>
        </small>
    </a>
  );
}
