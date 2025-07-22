import { useClickOutside } from 'hooks/useClickOutside';
import React, { useState } from 'react';
import { AnimatePresence, motion } from 'framer-motion';

export interface IDropdownProps {
    menuClass?: string;
    isNav?: boolean;
    buttonSnippet: React.ReactNode;
    children: React.ReactNode;
}

export function Dropdown(props: IDropdownProps) {
    const [isOpen, setIsOpen] = useState<boolean>(false);

    function onClick() {
        setIsOpen(!isOpen);
    }

    function closeDropdown() {
        setIsOpen(false);
    }

    function renderMenu() {
        return (
            <>
                {isOpen &&
                    <AnimatePresence>
                        <motion.ul className={`dropdown-menu show ${props.menuClass}`}
                            data-bs-popper="static"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            exit={{ opacity: 0 }}
                            transition={{ duration: 0.1 }}
                        >
                            {props.children}
                        </motion.ul>
                    </AnimatePresence>
                }
            </>
        );
    }

    return (
        <>
            {props.isNav ? (
                <li className="dropdown nav-item">
                    <button
                        className="dropdown-toggle nav-link text-start w-100"
                        onClick={onClick}
                        type="button"
                        aria-label="Button"
                        aria-expanded={isOpen}
                        ref={useClickOutside(closeDropdown)}
                    >
                        {props.buttonSnippet}
                    </button>
                    {renderMenu()}
                </li>
            ) : (
                <div className="btn-group">
                    <button
                        className="dropdown-toggle btn btn-outline-primary rounded-3 border-1"
                        onClick={onClick}
                        type="button"
                        aria-label="Button"
                        aria-expanded={isOpen}
                        ref={useClickOutside(closeDropdown)}
                    >
                        {props.buttonSnippet}
                    </button>
                    {renderMenu()}
                </div>
            )}
        </>
    );
}
