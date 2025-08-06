'use client';

import { useClickOutside } from '../../hooks/useClickOutside';
import React, { useState } from 'react';
import { AnimatePresence, motion } from 'framer-motion';
import styles from './Dropdown.module.scss';

export interface IDropdownProps {
    classes?: string;
    menuClass?: string;
    isNav?: boolean;
    buttonSnippet: React.ReactNode;
    hideChevron?: boolean;
    children: React.ReactNode;
}

export function Dropdown({
    classes = '',
    menuClass = '',
    isNav = false,
    buttonSnippet,
    hideChevron,
    children
}: IDropdownProps) {
    const [isOpen, setIsOpen] = useState<boolean>(false);

    function onClick() {
        setIsOpen(!isOpen);
    }

    function closeDropdown() {
        setIsOpen(false);
    }

    function renderMenu() {
        return (
            <AnimatePresence>
                <motion.ul className={`dropdown-menu ${styles['dropdown-menu']} ${isOpen && 'show'} ${menuClass}`}
                    data-bs-popper="static"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    exit={{ opacity: 0 }}
                    transition={{ duration: 0.1 }}
                >
                    {children}
                </motion.ul>
            </AnimatePresence>
        );
    }

    function renderToggler() {
        return (
            <div className="d-flex justify-content-between">
                {hideChevron ? (
                    buttonSnippet
                ) : (
                    <>
                        <div>{buttonSnippet}</div>
                        <div>
                            <i className="bi bi-chevron-down" style={{
                                transform: `rotate(${isOpen ? '-180' : '0'})`,
                                transition: 'transform ease-in-out .2s'
                            }} />
                        </div>
                    </>
                )}
            </div>
        );
    }

    return (
        <>
            {isNav ? (
                <li className={`dropdown nav-item ${classes}`}>
                    <button
                        className={`dropdown-toggle ${styles['dropdown-toggle']} nav-link text-start w-100`}
                        onClick={onClick}
                        type="button"
                        aria-label="Button"
                        aria-expanded={isOpen}
                        ref={useClickOutside(closeDropdown)}
                    >
                        {renderToggler()}
                    </button>
                    {renderMenu()}
                </li>
            ) : (
                <div className={`btn-group ${classes}`}>
                    <button
                        className={`dropdown-toggle ${styles['dropdown-toggle']} btn btn-outline-primary text-start rounded-3 border-1`}
                        onClick={onClick}
                        type="button"
                        aria-label="Button"
                        aria-expanded={isOpen}
                        ref={useClickOutside(closeDropdown)}
                    >
                        {renderToggler()}
                    </button>
                    {renderMenu()}
                </div>
            )}
        </>
    );
}
