import { generateSearchQuery } from '@/lib/gridUtil';
import DefaultGridState from '@/lib/types/DefaultGridState';
import GridState from '@/lib/types/GridState';
import { clamp } from '@/lib/util';
import Link from 'next/link';

const INNER_PAGINATION_LINKS_COUNT = 9;

export interface IPaginationProps {
    gridState: GridState;
    defaultState: DefaultGridState;
    totalItems: number;
    defaultLimit: number;
}

export function Pagination({
    gridState,
    defaultState,
    totalItems,
    defaultLimit
}: IPaginationProps) {

    let currentPage = gridState.p ?? 1;
    let totalPages =
        Math.ceil(totalItems / (gridState.limit ?? defaultLimit));
    let totalLinks =
        totalPages > INNER_PAGINATION_LINKS_COUNT ? INNER_PAGINATION_LINKS_COUNT : totalPages;
    let offsetStart =
        clamp(
            currentPage - INNER_PAGINATION_LINKS_COUNT / 2,
            1,
            totalPages - INNER_PAGINATION_LINKS_COUNT + 1
        );

    function getPageUrl(page: number): string {
        return generateSearchQuery('p', page, gridState, defaultState);
    }

    return (
        <nav aria-label="Pagination">
            <ul className="pagination">
                <li className="page-item">
                    <Link
                        className={`page-link ${currentPage === 1 && 'disabled'}`}
                        href={getPageUrl(currentPage - 1)}
                        tabIndex={currentPage === 1 ? -1 : undefined}
                    >
                        Previous
                    </Link>
                </li>

                {Array(totalLinks).fill(null).map((_, i) => {
                    const pageIndex = i + 1;
                    const page = offsetStart + i;

                    return <li className="page-item" key={pageIndex}>
                        <Link
                            className={`page-link ${page === currentPage && 'active'}`}
                            href={getPageUrl(pageIndex)}
                            aria-current={page === currentPage ? 'page' : undefined}
                        >
                            {((pageIndex == 2 && page > 2) || (pageIndex == INNER_PAGINATION_LINKS_COUNT - 1 && page < totalPages - 1)) ? (
                                '...'
                            ) : (
                                pageIndex
                            )}
                        </Link>
                    </li>;
                })}

                <li className="page-item">
                    <Link
                        className={`page-link ${currentPage === totalLinks && 'disabled'}`}
                        href={getPageUrl(currentPage + 1)}
                        tabIndex={currentPage === totalLinks ? -1 : undefined}
                    >
                        Next
                    </Link>
                </li>
            </ul>
        </nav>
    );
}
