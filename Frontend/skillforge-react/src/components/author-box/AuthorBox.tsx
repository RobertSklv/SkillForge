import moment from 'moment';
import { Icon } from '../icon/Icon';
import { getImagePath, formatRelativeTime } from '@/lib/util';
import Link from 'next/link';
import './_author-box.scss';

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
    classes,
    size = 'normal',
    indent = true,
}: IAuthorBoxProps) {
    return (
        <div className={`author-box d-flex gap-2 size-${size} ${classes} ${indent ? 'ms-2' : ''}`}>
            <div className={`author-box__image-wrapper d-flex flex-column justify-content-start align-items-end ${!indent ? 'w-auto' : ''}`}>
                <Link href={`/user/${name}`} className="text-decoration-none">
                    <img src={getImagePath(avatarImage)} className="author-box__image round-image" alt="Robert profile" />
                </Link>
            </div>
            <div className="author-box__info-col d-flex flex-column">
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
