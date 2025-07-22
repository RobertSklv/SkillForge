import { AnimatePresence, motion } from 'framer-motion';
import { useClickOutside } from 'hooks/useClickOutside';
import { useEffect, useState, type ReactNode } from 'react';
import { Link } from 'react-router-dom';

export interface INavbarProps {
    logoLink: string;
    logoSnippet: ReactNode;
    children?: ReactNode;
    linksSnippet?: () => ReactNode;
}

export function Navbar(props: INavbarProps) {
    const [isOpen, setIsOpen] = useState(false);
    const [windowWidth, setWindowWidth] = useState<number>(1400);
    const isHamburgerMenu = windowWidth <= 991;

    function onTogglerClick() {
        setIsOpen(!isOpen);
    }

    function closeMenu() {
        setIsOpen(false);
    }

    function updateWindowWidth() {
        setWindowWidth(window.innerWidth);
    }

    useEffect(() => {
        updateWindowWidth();

        window.addEventListener('resize', updateWindowWidth);

        return () => {
            window.removeEventListener('resize', updateWindowWidth);
        };
    });

    return (
        <nav className="navbar navbar-expand-lg fixed-top bg-primary navbar-main" ref={useClickOutside(closeMenu)}>
            <div className="container">
                <Link className="navbar-brand" to={props.logoLink} title="To homepage">
                    {props.logoSnippet}
                </Link>
                <div className="d-flex justify-content-center flex-shrink-1" style={{ flexGrow: 0.5 }}>
                    <div className="w-100">
                        {props.children}
                    </div>
                </div>
                <button
                    className={`navbar-toggler ${!isOpen ? 'collapsed' : ''}`}
                    onClick={onTogglerClick}
                    type="button"
                    aria-controlsName="navbarNavDropdown"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>

                <AnimatePresence>
                    {(!isHamburgerMenu || isOpen) &&
                        <motion.div
                            className={`collapse navbar-collapse justify-content-end flex-shrink-0 flex-grow-0
                                ${isHamburgerMenu
                                    ? 'position-absolute start-0 end-0 navbar bg-primary'
                                    : ''}
                                ${isOpen ? 'show' : ''}`}
                            style={{
                                top: isHamburgerMenu ? '54px' : 'auto'
                            }}
                            id="navbarNavDropdown"
                            initial="hidden"
                            animate="visible"
                            exit="hidden"
                            variants={{
                                hidden: { y: -15, opacity: 0.0 },
                                visible: { y: 0, opacity: 1.0 },
                            }}
                            transition={{ duration: 0.1 }}
                        >
                            <ul
                                className={`navbar-nav ${isHamburgerMenu
                                    ? 'container text-start justify-content-start align-items-start'
                                    : ''}`}
                                style={{
                                    paddingLeft: isHamburgerMenu ? '0.75rem' : 'inherit'
                                }}
                            >
                                {props.linksSnippet?.()}
                            </ul>
                        </motion.div>
                    }
                </AnimatePresence>
            </div>
        </nav>
    );
}
