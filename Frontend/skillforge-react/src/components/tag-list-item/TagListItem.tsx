'use client';

import { TagLink } from '../tag-link/TagLink';
import { FollowButton } from '../follow-button/FollowButton';
import TagListItemType from '@/lib/types/TagListItemType';
import { useCurrentUser } from '@/hooks/useCurrentUser';

export interface ITagListItemProps {
    data: TagListItemType;
    classes?: string;
}

export function TagListItem({
    data,
    classes = ''
}: ITagListItemProps) {
    const { currentUser } = useCurrentUser();

    return (
        <div className={`d-flex justify-content-center justify-content-lg-between align-items-center ${classes}`}>
            <TagLink data={data.Link} />
            {currentUser &&
                <FollowButton
                    subjectName={data.Link.Name}
                    type="tag"
                    isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
                    btnSize="sm"
                />
            }
        </div>
    );
}
