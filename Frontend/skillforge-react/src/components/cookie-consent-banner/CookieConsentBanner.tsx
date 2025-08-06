'use client';

import { Icon } from '../icon/Icon';
import { Button } from '../button/Button';
import { useEffect, useState } from 'react';
import { AnimatePresence, motion } from 'framer-motion';

export function CookieConsentBanner() {
    const [show, setShow] = useState<boolean>(false);

    function createCookie(consent: boolean) {
        document.cookie = `cookie_consent=${consent}; path=/; max-age=` + 60 * 60 * 24 * 365;
    }

    function close() {
        setShow(false);

        createCookie(false);
    }

    function accept() {
        setShow(false);

        createCookie(true);
    }

    useEffect(() => {
        if (!document.cookie.includes('cookie_consent')) {
            setTimeout(() => {
                setShow(true);
            }, 400);
        }
    }, []);

    return (
        <>
            <aside
                className={`cookie-consent-banner offcanvas offcanvas-bottom ${show ? 'show' : 'hiding'}`}
                tabIndex={-1}
                id="offcanvasBottom"
                aria-labelledby="offcanvasBottomLabel"
                aria-modal={show ? 'true' : undefined}
                role={show ? 'dialog' : undefined}
            >
                <div className="offcanvas-header">
                    <h2 className="offcanvas-title" id="offcanvasBottomLabel">
                        <Icon type="cookie" classes="me-2" />
                        Cookies
                    </h2>
                    <button type="button" className="btn-close" aria-label="Close" onClick={close}></button>
                </div>
                <div className="offcanvas-body">
                    <p>We value your privacy. We use cookies to improve your experience. Do you agree to our cookie policy?</p>
                </div>
                <div className="p-3 text-end">
                    <Button color="primary" size="lg" classes="me-3" onClick={accept}>Accept</Button>
                    <Button color="secondary" size="lg" onClick={close}>Deny</Button>
                </div>
            </aside>
            <AnimatePresence>
                {show && (
                    <motion.div
                        className="offcanvas-backdrop show"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 0.5 }}
                        exit={{ opacity: 0 }}
                        transition={{ duration: 0.15 }}
                    ></motion.div>
                )}
            </AnimatePresence>
        </>
    );
}
