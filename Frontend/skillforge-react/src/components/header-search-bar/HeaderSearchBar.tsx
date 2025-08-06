'use client';

import { ChangeEvent, FocusEvent, KeyboardEvent, useRef, useState } from 'react';
import { Icon } from '../icon/Icon';
import moment from 'moment';
import ArticleSearchItemType from '@/lib/types/ArticleSearchItemType';
import { searchArticles } from '@/lib/api/client';
import { useRouter } from 'next/navigation';
import { AnimatePresence, motion } from 'framer-motion';

export interface IHeaderSearchBarProps {
    classes?: string;
}

export function HeaderSearchBar({ classes = '' }: IHeaderSearchBarProps) {
    const [inputValue, setInputValue] = useState('');
    const inputValueRef = useRef(inputValue);
    const [suggestions, setSuggestions] = useState<ArticleSearchItemType[]>([]);
    const [showSuggestions, setShowSuggestions] = useState<boolean>(false);
    const router = useRouter();

    const suggestionsElement = useRef<HTMLDivElement>(null);

    async function updateSuggestions() {
        if (inputValueRef.current) {
            setSuggestions(await searchArticles(inputValueRef.current));
        } else {
            setSuggestions([]);
        }
    }

    function hideSuggestions() {
        setShowSuggestions(false);
    }

    function onChange(e: ChangeEvent<HTMLInputElement>) {
        setInputValue(e.target.value);
        inputValueRef.current = e.target.value;
        updateSuggestions();
    }

    function onFocus() {
        setShowSuggestions(true);
        updateSuggestions();
    }

    function onBlur(e: FocusEvent<HTMLInputElement, Element>) {
        if (!suggestionsElement.current?.contains(e.relatedTarget as Node)) {
            hideSuggestions();
        }
    }

    function search() {
        router.push(`/search?q=${encodeURIComponent(inputValueRef.current)}`);
    }

    function onKeyDown(e: KeyboardEvent<HTMLInputElement>) {
        if (e.code === 'Enter') {
            if (inputValueRef.current.length) {
                search();
            }
        }
    }

    return (
        <div className={`position-relative ${classes}`}>
            <div className="input-group search-bar mx-auto my-0">
                <input
                    id="search"
                    type="search"
                    className="form-control rounded-start-3"
                    placeholder="Search articles..."
                    aria-label="Search articles..."
                    aria-describedby="search_button"
                    autoComplete="off"
                    value={inputValue}
                    onChange={onChange}
                    onFocus={onFocus}
                    onBlur={onBlur}
                    onKeyDown={onKeyDown}
                />
                <button className="btn btn-light rounded-end-3" type="button" id="search_button" onClick={search} disabled={!inputValue}>
                    <Icon type="search" classes="d-block d-sm-none" />
                    <span className="d-none d-sm-block">Search</span>
                </button>
            </div>
            {(showSuggestions && !!suggestions.length) &&
                <AnimatePresence>
                    <motion.div
                        className="position-absolute w-100 z-3 mt-1 shadow"
                        ref={suggestionsElement}
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                        transition={{ duration: 0.1 }}
                    >
                        <ul className="list-group rounded-3">
                            {suggestions.map(item => (
                                <a
                                    key={item.ArticleId}
                                    href={`/article/${item.ArticleId}`}
                                    onClick={hideSuggestions}
                                    className="list-group-item list-group-item-action d-flex justify-content-between align-items-start"
                                >
                                    <div className="ms-2 me-auto">
                                        <h4 className="h6" style={{ marginLeft: '-4px' }}>
                                            {item.Title}
                                        </h4>
                                        <small className="text-body-tertiary">@{item.AuthorName}</small>
                                        <small className="ms-2 text-muted">{moment(item.DatePosted).format('MMM Do YY')}</small>
                                    </div>
                                </a>
                            ))}
                        </ul>
                    </motion.div>
                </AnimatePresence>
            }
        </div>
    );
}
