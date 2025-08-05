import GridState from "@/lib/types/GridState";

export interface ISearchBarProps {
    gridState: GridState;
}

export function SearchBar({
    gridState
}: ISearchBarProps) {
    return (
        <form method="GET">
            <div className="input-group">
                <input
                    id="search"
                    name="q"
                    type="search"
                    className="form-control rounded-start-3"
                    placeholder="Search articles..."
                    aria-label="Search articles..."
                    aria-describedby="search_button"
                    autoComplete="off"
                />
                <button className="btn btn-light rounded-end-3" type="submit" id="search_button">Search</button>
            </div>
            {gridState.sortBy && <input type="hidden" name="sortBy" value={gridState.sortBy} />}
            {gridState.sortOrder && <input type="hidden" name="sortOrder" value={gridState.sortOrder} />}
        </form>
    );
}
