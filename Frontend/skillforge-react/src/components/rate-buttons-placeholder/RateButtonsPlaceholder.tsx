import * as React from 'react';
import { Icon } from '../icon/Icon';
import styles from './RateButtonsPlaceholder.module.css';

export interface IRateButtonsPlaceholderProps {
    size?: 'normal' | 'small';
}

export function RateButtonsPlaceholder({ size }: IRateButtonsPlaceholderProps) {
    return (
        <div className="text-end d-flex">
            <div className={`${styles.rateBtn} text-primary d-flex align-items-center mx-1`}>
                <Icon type="hand-thumbs-up" classes={size === 'normal' ? 'fs-5' : 'fs-6'} />
                <span className="placeholder placeholder-sm col-4 ms-1 h-auto"></span>
            </div>
            <div className={`${styles.rateBtn} text-primary d-flex align-items-center mx-1`}>
                <Icon type="hand-thumbs-down" classes={size === 'normal' ? 'fs-5' : 'fs-6'} />
                <span className="placeholder placeholder-sm col-4 ms-1 h-auto"></span>
            </div>
        </div>
    );
}
