import { GutterLevel } from '@/lib/types/GutterLevel';
import PaginationResponse from '@/lib/types/PaginationResponse';
import { ReactNode } from 'react';

export interface IGridProps<T> {
	data: PaginationResponse<T>;
	cols: number;
	mdCols: number;
	lgCols: number;
	gap?: GutterLevel;
	item: (item: T) => ReactNode;
	getItemKey: (item: T) => any;
	header?: ReactNode;
	footer?: ReactNode;
}

export function Grid<T>({
	data,
	cols,
	mdCols,
	lgCols,
	gap,
	item,
	getItemKey,
	header,
	footer
}: IGridProps<T>) {
	const colClasses = `col-${getColumnSize(cols)} col-md-${getColumnSize(mdCols)} col-lg-${getColumnSize(lgCols)}`;

	function getColumnSize(columnsCount: number): number {
		return 12 / columnsCount;
	}

	return (
		<>
			{header}
			<div className="row">
				{data.Items.map(i => (
					<div className={`${colClasses} p-${gap}`} key={getItemKey(i)}>
						{item(i)}
					</div>
				))}
			</div>
			{footer}
		</>
	);
}
