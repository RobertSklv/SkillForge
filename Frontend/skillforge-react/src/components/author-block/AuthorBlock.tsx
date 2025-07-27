'use client';

import { Block } from '@/components/block/Block';
import { FollowButton } from '@/components/follow-button/FollowButton';
import { useCurrentUser } from '../../hooks/useCurrentUser';
import moment from 'moment';
import type Author from 'skillforge-common/types/Author';
import { getImagePath } from 'skillforge-common/util';

export interface IAuthorBlockProps {
    data: Author;
    classes?: string;
}

export function AuthorBlock({ data, classes }: IAuthorBlockProps) {
    const { currentUser } = useCurrentUser();

    return (
        <Block classes={classes} header={<h2 className="text-center">Author</h2>}>
            <div className="card-body">
                <div className="p-4 text-center">
                    <a href={`/user/${data.Link.Name}`}>
                        <img
                            src={getImagePath(data.Link.AvatarImage)}
                            alt={`${data.Link.Name} avatar`}
                            className="rounded-circle w-100 object-fit-cover border border-2 border-primary mb-4"
                            style={{
                                aspectRatio: 1,
                                maxWidth: '300px'
                            }}
                        />
                    </a>
                </div>
                <dl className="mb-0 text-center text-lg-start">
                    <dt>Nickname</dt>
                    <dd className="text-body-tertiary">
                        {data.Link.Name}
                    </dd>
                    {data.Bio && (
                        <>
                            <dt>Bio</dt>
                            <dd className="text-body-tertiary">{data.Bio}</dd>
                        </>
                    )}
                    <dt>Date joined</dt>
                    <dd className="text-body-tertiary">
                        {moment(data.JoinedDate).format('D MMMM YYYY')}
                    </dd>
                </dl>
                {(currentUser && currentUser.Name != data.Link.Name) &&
                    <div className="text-center mt-5">
                        <FollowButton
                            subjectName={data.Link.Name}
                            type="user"
                            isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
                        />
                    </div>
                }
            </div>
        </Block>
    );
}
