import { Button } from '@/components/button/Button';
import { followTag, followUser, unfollowTag, unfollowUser } from '@/lib/api/client';

export interface IFollowButtonProps {
    subjectName: string;
    type: 'user' | 'tag';
    isFollowedByCurrentUser?: boolean;
    setIsFollowedByCurrentUser?: (f: boolean) => void;
    btnSize?: 'md' | 'sm';
}

export function FollowButton({ subjectName, type, isFollowedByCurrentUser, setIsFollowedByCurrentUser, btnSize = 'md' }: IFollowButtonProps) {
    const followFunctions = {
        user: followUser,
        tag: followTag
    };

    const unfollowFunctions = {
        user: unfollowUser,
        tag: unfollowTag
    };

    async function follow() {
        await followFunctions[type](subjectName);

        setIsFollowedByCurrentUser?.(true);
    }

    async function unfollow() {
        await unfollowFunctions[type](subjectName);

        setIsFollowedByCurrentUser?.(false);
    }

    return (
        <>
            {isFollowedByCurrentUser ? (
                <Button onClick={unfollow} size={btnSize} classes="follow-button" isOutline>Unfollow</Button>
            ) : (
                <Button onClick={follow} size={btnSize} classes="follow-button border border-2 border-primary">Follow</Button>
            )}
        </>
    );
}
