import { ArticleCard } from '@/components/article-card/ArticleCard';
import { SearchBar } from '@/components/grid/filters/search-bar/SearchBar';
import { SortByDropdown } from '@/components/grid/filters/sort-by-dropdown/SortByDropdown';
import { SortOrderDropdown } from '@/components/grid/filters/sort-order-dropdown/SortOrderDropdown';
import { Grid } from '@/components/grid/Grid';
import { Pagination } from '@/components/pagination/Pagination';
import { searchArticlesAdvanced } from '@/lib/api/client';
import ArticleCardType from '@/lib/types/ArticleCardType';
import DefaultGridState from '@/lib/types/DefaultGridState';
import GridState from '@/lib/types/GridState';
import { Metadata } from 'next';

const DEFAULT_LIMIT = 9;

const defaultGridState: DefaultGridState = {
    p: 1,
    limit: DEFAULT_LIMIT,
    q: undefined,
    sortBy: 'date',
    sortOrder: 'desc'
};

export interface ISearchPageProps {
    searchParams: GridState;
}

export async function generateMetadata({ searchParams }: ISearchPageProps): Promise<Metadata> {
    return {
        title: `SkillForge | Search: '${searchParams.q}'`,
        description: `Search results for: '${searchParams.q}'`,
        robots: 'noindex,nofollow',
        alternates: {
            canonical: `${process.env.NEXT_PUBLIC_BASE_URL}/search`
        }
    };
}

export default async function SearchPage({ searchParams }: ISearchPageProps) {
    let gridState: GridState = {
        p: searchParams.p,
        limit: searchParams.limit,
        q: searchParams.q,
        sortBy: searchParams.sortBy,
        sortOrder: searchParams.sortOrder,
    };

    let response = await searchArticlesAdvanced(gridState);

    async function header() {
        'use server';

        return (
            <>
                <div className="row mb-5">
                    <div className="col-12 col-md-6 mb-4 mb-md-0">
                        <SearchBar gridState={gridState} />
                    </div>

                    <div className="col-6 col-md-3">
                        <SortByDropdown gridState={gridState} defaultState={defaultGridState} />
                    </div>

                    <div className="col-6 col-md-3">
                        <SortOrderDropdown gridState={gridState} defaultState={defaultGridState} />
                    </div>
                </div>
                {response.TotalItems > DEFAULT_LIMIT &&
                    <div className="d-flex justify-content-center mb-3">
                        <Pagination
                            gridState={gridState}
                            defaultState={defaultGridState}
                            totalItems={response.TotalItems}
                            defaultLimit={DEFAULT_LIMIT}
                        />
                    </div>
                }
            </>
        );
    }

    async function footer() {
        'use server';

        return (
            <>
                {response.TotalItems > DEFAULT_LIMIT &&
                    <div className="d-flex justify-content-center mt-3">
                        <Pagination
                            gridState={gridState}
                            defaultState={defaultGridState}
                            totalItems={response.TotalItems}
                            defaultLimit={DEFAULT_LIMIT}
                        />
                    </div>
                }
            </>
        );
    }

    return (
        <>
            <div className="mb-5">
                <h1>Search results for '{gridState?.q}':</h1>
                <p className="text-body-tertiary h5">Found: {response.TotalItems}</p>
            </div>

            <Grid<ArticleCardType>
                data={response}
                cols={1}
                mdCols={2}
                lgCols={3}
                gap={3}
                header={header()}
                footer={footer()}
                item={item => <ArticleCard data={item} showComments={false} classes="h-100" />}
                getItemKey={item => item.ArticleId}
            />
        </>
    );
}
