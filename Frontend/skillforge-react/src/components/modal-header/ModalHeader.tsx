import { useModalContext } from '@/context/ModalContext';

export interface IModalHeaderProps {
    title?: string;
}

export function ModalHeader({ title }: IModalHeaderProps) {
    const modalContext = useModalContext();

    return (
        <div className="modal-header">
            <h2 className="modal-title fs-5">{title}</h2>
            <button type="button" className="btn-close" aria-label="Close" onClick={modalContext.close}></button>
        </div>
    );
}
