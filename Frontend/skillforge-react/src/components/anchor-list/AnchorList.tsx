import { Block } from '@components/block/Block';
import { type ReactNode } from 'react';

export interface IAnchorListProps<T> {
    title: string;
    items: T[];
    itemSnippet: (item: T) => ReactNode;
}

export function AnchorList<T> ({ title, items, itemSnippet }: IAnchorListProps<T>) {
  return (
    <Block header={<h3 className="card-title h4">{title}</h3>}>
        <div className="card-body">
            <ul className="list-group rounded-3">
                {items.map(itemSnippet)}
            </ul>
        </div>
    </Block>
  );
}
