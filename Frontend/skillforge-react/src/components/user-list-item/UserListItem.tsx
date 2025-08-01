'use client';

import { UserLink } from '../user-link/UserLink';
import { FollowButton } from '../follow-button/FollowButton';
import UserListItemType from '@/lib/types/UserListItemType';
import { useCurrentUser } from '@/hooks/useCurrentUser';

export interface IUserListItemProps {
    data: UserListItemType;
    classes?: string;
}

export function UserListItem({
    data,
    classes = ''
}: IUserListItemProps) {
    const { currentUser } = useCurrentUser();

    return (
        <div className={`d-flex justify-content-center justify-content-lg-between align-items-center ${classes}`}>
            <UserLink data={data.Link} />
            {(currentUser && currentUser.Name != data.Link.Name) &&
                <FollowButton
                    subjectName={data.Link.Name}
                    type="user"
                    isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
                    btnSize="sm"
                />
            }
        </div>
    );
}
