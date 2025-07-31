import { Icon } from '@/components/icon/Icon';
import { useFormContext } from '@/context/FormContext';
import { AnimatePresence, motion } from 'framer-motion';
import { FocusEvent, KeyboardEvent, useEffect, useRef, useState } from 'react';
import { FieldValues, RegisterOptions } from 'react-hook-form';
import { searchTags } from '@/lib/api/client';
import TagLinkType from '@/lib/types/TagLinkType';

export interface ITagComboBoxProps {
    id: string;
    label?: string;
    tags?: string[];
    onTagsChange?: (tags: string[]) => void;
    options?: RegisterOptions<FieldValues, string> | undefined;
    limit?: number;
}

export function TagComboBox({
    id,
    label = id,
    tags = [],
    onTagsChange,
    options,
    limit = 3
}: ITagComboBoxProps) {
    const formContext = useFormContext();
    const isInvalid = !!formContext?.form.formState.errors[id];

    const [inputValue, setInputValue] = useState<string>('');
    const [showSuggestions, setShowSuggestions] = useState<boolean>(false);
    const [suggestions, setSuggestions] = useState<TagLinkType[]>([]);
    const isWithinLimit = !tags || tags.length < limit;

    const suggestionsElement = useRef<HTMLDivElement>(null);

    async function updateSuggestions() {
        if (isWithinLimit) {
            setSuggestions(await searchTags(inputValue, tags));

            if (inputValue && !suggestions.map((s) => s.Name).includes(inputValue)) {
                suggestions.unshift({
                    Name: inputValue
                });
            }
        }
    }

    function onInput() {
        updateSuggestions();
    }

    function onFocus() {
        setShowSuggestions(true);
        updateSuggestions();
    }

    function onBlur(event: FocusEvent<HTMLInputElement, Element>) {
        if (suggestionsElement?.current && !suggestionsElement.current.contains(event.relatedTarget as Node)) {
            setShowSuggestions(false);
        }
    }

    function onKeyDown(event: KeyboardEvent<HTMLInputElement>) {
        if (event.code === 'Enter') {
            if (inputValue.length) {
                addTag(inputValue);
            }
        } else if (event.code === 'Backspace') {
            if (!inputValue.length) {
                let lastTag = tags.pop();
                onChange(tags);

                if (lastTag) {
                    setInputValue(lastTag);
                    updateSuggestions();
                }
            }
        }
    }

    function addTag(tagName: string) {
        tags ??= [];

        if (!isWithinLimit || isInvalid) {
            return;
        }

        if (!tags.includes(tagName)) {
            tags.push(tagName);
        }

        onChange(tags);
        setInputValue('');
        setShowSuggestions(false);
        updateSuggestions();
    }

    function removeTag(tagName: string) {
        tags = tags.filter((t) => t != tagName);

        onChange(tags);
        updateSuggestions();
    }

    function onChange(tags: string[]) {
        onTagsChange?.(tags);
        formContext.form.setValue(id, tags, {
            shouldValidate: true
        });
    }

    useEffect(() => {
        formContext.form.register(id, options);

        return () => {
            formContext.form.unregister(id);
        }
    }, [formContext.form.register]);

    return (
        <div className="combo-box position-relative mb-4">
            <label htmlFor={id} className="form-label">{label}</label>
            <div className={`d-flex flex-wrap gap-1 ${isInvalid ? 'is-invalid' : ''}`}>
                {tags.map(tag => {
                    return (
                        <div className="toast show w-auto" key={tag}>
                            <div className="d-flex">
                                <div className="toast-body p-2">
                                    <strong>#{tag}</strong>
                                </div>
                                <button
                                    type="button"
                                    className="btn-close me-2 m-auto"
                                    aria-label="Close"
                                    onClick={() => removeTag(tag)}
                                ></button>
                            </div>
                        </div>
                    );
                })}
                <input
                    {...formContext?.form.register(id, {
                        ...options,
                        onChange: e => setInputValue(e.target.value)
                    })}
                    id={id}
                    type="text"
                    className={`form-control flex-grow-1 w-auto ${isInvalid ? 'is-invalid' : ''}`}
                    onFocus={onFocus}
                    onBlur={onBlur}
                    onInput={onInput}
                    onKeyDown={onKeyDown}
                    value={inputValue}
                />
            </div>
            {(showSuggestions && !!suggestions.length && isWithinLimit && !isInvalid) &&
                <AnimatePresence>
                    <motion.div
                        className="position-absolute w-100 z-3 mt-1 shadow"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                        transition={{ duration: 0.1 }}
                        ref={suggestionsElement}
                    >
                        <ul className="list-group rounded-3">
                            {suggestions.map(tag => {
                                return (
                                    <button
                                        key={tag.Name}
                                        type="button"
                                        onClick={() => addTag(tag.Name)}
                                        className="list-group-item list-group-item-action d-flex justify-content-between align-items-start"
                                    >
                                        <div className="ms-2 me-auto">
                                            <div className="fw-bold" style={{
                                                marginLeft: '-4px'
                                            }}>
                                                <Icon type="hash" />{tag.Name}
                                            </div>
                                            <small className="text-body-tertiary">{tag.Description}</small>
                                        </div>
                                    </button>
                                );
                            })}
                        </ul>
                    </motion.div>
                </AnimatePresence>
            }
            <div className="invalid-feedback">
                {isInvalid && formContext?.form.formState.errors[id]?.message as string}
            </div>
        </div>
    );
}
