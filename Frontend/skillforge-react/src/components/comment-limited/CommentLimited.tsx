import { AuthorBox } from '@/components/author-box/AuthorBox';
import type CommentModel from 'skillforge-common/types/CommentModel';

export interface ICommentLimitedProps {
    data: CommentModel
}

export function CommentLimited({ data }: ICommentLimitedProps) {
    return (
        <>
            <AuthorBox
                name={data.User.Name}
                avatarImage={data.User.AvatarImage}
                date={data.DateWritten}
                editedDate={data.DateEdited}
                size="small"
            />
            <div className="mx-5 p-3">
                <div className="content-wrapper rounded-3 p-3">
                    <div className="content text-break rich-text-content" dangerouslySetInnerHTML={{ __html: data.Content }}></div>
                </div>
            </div>
        </>
    );
}
