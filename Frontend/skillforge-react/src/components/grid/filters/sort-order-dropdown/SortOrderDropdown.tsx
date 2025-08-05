import { DropdownItem } from '@/components/dropdown-item/DropdownItem';
import { Dropdown } from '@/components/dropdown/Dropdown';
import { generateSearchQuery } from '@/lib/gridUtil';
import DefaultGridState from '@/lib/types/DefaultGridState';
import GridState from '@/lib/types/GridState';

export interface ISortOrderDropdownProps {
    gridState: GridState;
    defaultState: DefaultGridState;
}

export function SortOrderDropdown({
    gridState,
    defaultState
}: ISortOrderDropdownProps) {
    return (
        <Dropdown classes='w-100' menuClass='w-100' buttonSnippet={`Order: ${gridState.sortOrder === 'asc' ? 'Ascending' : 'Descending'}`}>
            <DropdownItem href={generateSearchQuery('sortOrder', 'desc', gridState, defaultState)}>Descending</DropdownItem>
            <DropdownItem href={generateSearchQuery('sortOrder', 'asc', gridState, defaultState)}>Ascending</DropdownItem>
        </Dropdown>
    );
}
