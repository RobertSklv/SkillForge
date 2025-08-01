import { Link } from '@/components/link/Link';
import { BootstrapColor } from '@/lib/types/BootstrapColor';
import UserLinkType from '@/lib/types/UserLinkType';
import { getImagePath } from '@/lib/util';
import styles from './UserLink.module.scss';

export interface IUserLinkProps {
    data: UserLinkType;
    size?: 'normal' | 'small';
    background?: 'outline' | 'fill';
    color?: BootstrapColor;
    muted?: boolean;
}

export function UserLink({
    data,
    size,
    background,
    color,
    muted
}: IUserLinkProps) {
    return (
        <Link
            href={`/user/${data.Name}`}
            size={size}
            background={background}
            color={color}
            borderRadius="pill"
            classes={styles['user-link']}
            muted={muted}
        >
            <div className="d-flex align-items-center">
                <img
                    src={`${getImagePath(data.AvatarImage)}`}
                    className={`${styles['user-link__image']} border border-1 round-image me-1`}
                    alt={`${data.Name} avatar`}
                />
                <span className="text-truncate">{data.Name}</span>
            </div>
        </Link>
    );
}
