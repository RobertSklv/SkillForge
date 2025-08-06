import { Block } from '../block/Block';

export interface IOutOfArticlesBlockProps {
	message?: string;
}

export function OutOfArticlesBlock({
	message = "You've reached the end. No more articles to show."
}: IOutOfArticlesBlockProps) {

	return (
		<Block>
			<div className="card-body d-flex align-items-center p-3 px-4 p-md-4 px-md-5">
				<span className="display-1 me-4" style={{
					marginTop: '-14px',
					minWidth: '30px'
				}}>:(</span>
				<span>{message}</span>
			</div>
		</Block>
	);
}
