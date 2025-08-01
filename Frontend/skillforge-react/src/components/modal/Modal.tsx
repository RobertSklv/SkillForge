import { ReactNode } from "react";
import { Scrollable } from "../scrollable/Scrollable";
import { useClickOutside } from "@/hooks/useClickOutside";
import { AnimatePresence, motion } from "framer-motion";
import { ModalContext } from "@/context/ModalContext";
import styles from './Modal.module.scss';

export interface IModalProps {
    children?: ReactNode;
    header?: ReactNode;
    footer?: ReactNode;
    show: boolean;
    setShow: (show: boolean) => void;
    size?: 'default' | 'sm' | 'lg' | 'xl';
    verticallyCentered?: boolean;
    scrollable?: boolean;
}

export function Modal({
    children,
    header,
    footer,
    show,
    setShow,
    size = 'default',
    verticallyCentered,
    scrollable
}: IModalProps) {
    function close() {
        setShow(false);
    }

    const clickOutsideRef = useClickOutside<HTMLDivElement>(close);

    if (!show) return null;

    return (
        <AnimatePresence>
            <motion.aside
                key="modal"
                className="modal fade show d-block"
                tabIndex={-1}
                aria-modal={show ? 'true' : undefined}
                role={show ? 'dialog' : undefined}
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                transition={{ duration: 0.15 }}
            >
                <ModalContext.Provider value={{
                    close
                }}>
                    <div
                        className={`${styles['modal-dialog']} modal-dialog modal-fullscreen-sm-down ${size === 'default' ? '' : `modal-${size}`} ${verticallyCentered ? 'modal-dialog-centered' : ''} ${scrollable ? `modal-dialog-scrollable ${styles['modal-dialog-scrollable']}` : ''}`}
                        ref={clickOutsideRef}
                    >
                        <div className="modal-content">
                            {header}
                            {scrollable ? (
                                <Scrollable className={`modal-body ${styles['modal-body']}`}>
                                    {children}
                                </Scrollable>
                            ) : (
                                <div className={`modal-body ${styles['modal-body']}`}>
                                    {children}
                                </div>
                            )}
                            {footer &&
                                <div className="modal-footer">
                                    {footer}
                                </div>
                            }
                        </div>
                    </div>
                </ModalContext.Provider>
            </motion.aside>
            <div className="modal-backdrop show"></div>
        </AnimatePresence>
    );
}
