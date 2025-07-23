import { motion, AnimatePresence } from 'framer-motion';
import * as React from 'react';

export interface IBlockProps {
    classes?: string;
    children?: React.ReactNode;
    header?: React.ReactNode;
    footer?: React.ReactNode;
}

export function Block(props: IBlockProps) {
    const BORDER_RADIUS: 1 | 2 | 3 | 4 | 5 = 3;

    return (
        <AnimatePresence>
            <motion.section
                className={`card border-dark border-1 rounded-${BORDER_RADIUS} bg-dark ${props.classes}`}
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                transition={{ duration: 0.12 }}
            >
                {props.header &&
                    <div className={`card-header border-bottom-0 rounded-top-${BORDER_RADIUS}`}>
                        {props.header}
                    </div>
                }

                {props.children}

                {props.footer &&
                    <div className={`card-footer border-top-0 rounded-bottom-${BORDER_RADIUS}`}>
                        {props.footer}
                    </div>
                }
            </motion.section>
        </AnimatePresence>
    );
}
