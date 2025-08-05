import moment from 'moment';
import { Icon } from '../icon/Icon';
import { getImagePath, formatRelativeTime } from '@/lib/util';
import Link from 'next/link';
import styles from './AuthorBox.module.scss';
import Image from 'next/image';

export interface IAuthorBoxProps {
    name: string;
    avatarImage: string | null | undefined;
    date: string;
    editedDate?: string;
    classes?: string;
    size?: 'normal' | 'small';
    indent?: boolean;
}

export function AuthorBox({
    name,
    avatarImage,
    date,
    editedDate,
    classes = '',
    size = 'normal',
    indent = true,
}: IAuthorBoxProps) {
    const imgSize = size === 'normal' ? 40 : 30;

    return (
        <div className={`${styles['author-box']} d-flex gap-2 ${styles[`size-${size}`]} ${classes} ${indent ? 'ms-2' : ''}`}>
            <div className={`${styles['author-box__image-wrapper']} d-flex flex-column justify-content-start align-items-end ${!indent ? 'w-auto' : ''}`}>
                <Link href={`/user/${name}`} className="text-decoration-none">
                    <Image
                        src={getImagePath(avatarImage)}
                        className={`${styles['author-box__image']} round-image`}
                        alt={`${name} profile`}
                        width={imgSize}
                        height={imgSize}
                    />
                </Link>
            </div>
            <div className={`${styles['author-box__info-col']} d-flex flex-column`}>
                <Link href={`/user/${name}`} className="text-decoration-none">{name}</Link>
                <div className="small text-tertiary">
                    <time dateTime={moment(date).format('YYYY-MM-DD HH:mm')}>
                        {moment(date).format('ddd, D MMMM, YYYY HH:mm')}
                    </time>
                    <span className="text-muted ms-2">({formatRelativeTime(date)})</span>
                </div>
                {editedDate &&
                    <div className="small text-info-emphasis">
                        <Icon type="pencil-fill" />
                        <time dateTime={moment(editedDate).format('YYYY-MM-DD HH:mm')}>
                            {moment(editedDate).format('ddd, D MMMM, YYYY HH:mm')}
                        </time>
                        <span className="ms-2">({formatRelativeTime(editedDate)})</span>
                    </div>
                }
            </div>
        </div>
    );
}
