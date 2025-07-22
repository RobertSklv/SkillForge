import { Icon } from '@components/icon/Icon';
import { AnimatePresence, motion } from 'framer-motion';
import { useToast } from 'hooks/useToast';

const iconMap = {
    success: 'check-circle-fill',
    warning: 'exclamation-triangle-fill',
    danger: 'x-circle-fill',
    info: 'info-circle-fill'
};

export function ToastContainer() {
    const { toasts, removeToast } = useToast();

    return (
        <div className="position-fixed pe-none start-0 bottom-0 end-0 p-3 z-3 d-flex flex-column-reverse align-items-end gap-2">
            <AnimatePresence>
                {toasts.map((toast) => {
                    return (
                        <motion.div
                            key={toast.uid}
                            className={`alert alert-${toast.type} alert-dismissible d-flex align-items-center w-auto pe-auto`}
                            role="alert"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            exit={{ opacity: 0 }}
                            transition={{ duration: 0.2 }}
                        >
                            <Icon type={iconMap[toast.type]} classes="me-2" />
                            <div>{toast.message}</div>
                            <button
                                type="button"
                                className="btn-close"
                                aria-label="Close"
                                onClick={() => removeToast(toast.uid)}
                            ></button>
                        </motion.div>
                    );
                })}
            </AnimatePresence>
        </div>
    );
}
