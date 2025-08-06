import * as React from 'react';
import { AuthorBoxPlaceholder } from '../author-box-placeholder/AuthorBoxPlaceholder';
import styles from './CommentLimitedPlaceholder.module.css';

export function CommentLimitedPlaceholder() {
	return (
		<>
			<AuthorBoxPlaceholder size="small" />
			<div className="mx-5 p-3">
				<div className={`${styles['content-wrapper']} rounded-3 p-3 placeholder col-12`}></div>
			</div>
		</>
	);
}
