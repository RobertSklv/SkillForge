import moment from 'moment';
import { Link } from 'react-router-dom';
import { Icon } from '../icon/Icon';
import { getImagePath, formatRelativeTime } from 'skillforge-common/util';

export interface IAuthorBoxProps {
    name: string;
    avatarImage: string | null | undefined;
    date: string;
    editedDate?: string;
    classes?: string;
    size?: 'normal' | 'small';
    indent?: boolean;
}

export function AuthorBox(props: IAuthorBoxProps) {
    return (
        <div className={`author-box d-flex gap-2 size-${props.size} ${props.classes} ${props.indent ? 'ms-2' : ''}`}>
            <div className={`author-box__image-wrapper d-flex flex-column justify-content-start align-items-end ${!props.indent ? 'w-auto' : ''}`}>
                <Link to={`/user/${props.name}`} className="text-decoration-none">
                    <img src={getImagePath(props.avatarImage)} className="author-box__image round-image" alt="Robert profile" />
                </Link>
            </div>
            <div className="author-box__info-col d-flex flex-column">
                <Link to={`/user/${props.name}`} className="text-decoration-none">{props.name}</Link>
                <div className="small text-tertiary">
                    <time dateTime={moment(props.date).format('YYYY-MM-DD HH:mm')}>
                        {moment(props.date).format('ddd, D MMMM, YYYY HH:mm')}
                    </time>
                    <span className="text-muted ms-2">({formatRelativeTime(props.date)})</span>
                </div>
                {props.editedDate &&
                    <div className="small text-info-emphasis">
                        <Icon type="pencil-fill" />
                        <time dateTime={moment(props.editedDate).format('YYYY-MM-DD HH:mm')}>
                            {moment(props.editedDate).format('ddd, D MMMM, YYYY HH:mm')}
                        </time>
                        <span className="ms-2">({formatRelativeTime(props.editedDate)})</span>
                    </div>
                }
            </div>
        </div>
    );
}
