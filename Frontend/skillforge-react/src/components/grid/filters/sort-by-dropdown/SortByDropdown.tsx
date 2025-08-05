import { DropdownItem } from '@/components/dropdown-item/DropdownItem';
import { Dropdown } from '@/components/dropdown/Dropdown';
import { generateSearchQuery } from '@/lib/gridUtil';
import DefaultGridState from '@/lib/types/DefaultGridState';
import GridState from '@/lib/types/GridState';

const labelMap: { [key: string]: string; } = {
    date: 'Date published',
    rating: 'Rating',
    views: 'Views',
};

export interface ISortByDropdownProps {
    gridState: GridState;
    defaultState: DefaultGridState;
}

export function SortByDropdown({
    gridState,
    defaultState
}: ISortByDropdownProps) {
    return (
        <Dropdown classes='w-100' menuClass='w-100' buttonSnippet={`Sort by: ${labelMap[gridState.sortBy ?? 'date']}`}>
            <DropdownItem href={generateSearchQuery('sortBy', 'date', gridState, defaultState)}>{labelMap.date}</DropdownItem>
            <DropdownItem href={generateSearchQuery('sortBy', 'rating', gridState, defaultState)}>{labelMap.rating}</DropdownItem>
            <DropdownItem href={generateSearchQuery('sortBy', 'views', gridState, defaultState)}>{labelMap.views}</DropdownItem>
        </Dropdown>
    );
}
