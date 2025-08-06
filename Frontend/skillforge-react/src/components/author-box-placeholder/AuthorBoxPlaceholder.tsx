import { style } from 'framer-motion/client';
import styles from './AuthorBoxPlaceholder.module.scss';

export interface IAuthorBoxPlaceholderProps {
    classes?: string;
    size?: 'normal' | 'small';
    indent?: boolean;
}

export function AuthorBoxPlaceholder({
    classes,
    size = 'normal',
    indent = true,
}: IAuthorBoxPlaceholderProps) {
    return (
        <div className={`${styles['author-box']} d-flex gap-2 size-${size} w-100 ${classes} ${indent ? 'ms-2' : ''}`}>
            <div className={`${styles['author-box__image-wrapper']} d-flex flex-column justify-content-start align-items-end ${!indent ? 'w-auto' : ''}`}>
                <span className={`${styles['author-box__image']} rounded-circle placeholder`}></span>
            </div>
            <div className={`${styles['author-box__info-col']} d-flex flex-column w-50`}>
                <span className={`${size == 'normal' ? 'fs-4' : 'fs-5'} placeholder col-4`}></span>
                <div className="small text-tertiary">
                    <span className="fs-6 placeholder col-5"></span>
                    <span className="text-muted ms-2 placeholder col-3"></span>
                </div>
            </div>
        </div>
    );
}
