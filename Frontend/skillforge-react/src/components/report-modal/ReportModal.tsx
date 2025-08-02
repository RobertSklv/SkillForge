import { Modal } from '../modal/Modal';
import { ModalHeader } from '../modal-header/ModalHeader';
import { Button } from '../button/Button';
import { Form } from '../form/Form';
import { RadioGroup } from '../form/radio-group/RadioGroup';
import { RadioField } from '../form/radio-field/RadioField';
import { useReportFormOptions } from '@/hooks/useReportFormOptions';
import { useState } from 'react';
import { useToast } from '@/hooks/useToast';
import ReportFormData from '@/lib/types/ReportFormData';
import { TextAreaField } from '../form/text-area-field/TextAreaField';

export interface IReportModalProps {
    title: string;
    entityId?: number;
    entityName?: string;
    action: string;
    show: boolean;
    setShow: (show: boolean) => void;
}

export function ReportModal({
    title,
    entityId,
    entityName,
    action,
    show,
    setShow,
}: IReportModalProps) {
    const { reportOptions } = useReportFormOptions();
    const { addToast } = useToast();

    const [reason, setReason] = useState<number>();
    const [message, setMessage] = useState<string>('');

    function onSubmit(data: ReportFormData) {
        data.Id = entityId;
        data.Name = entityName;

        return data;
    }

    function onSuccess() {
        addToast('Report submitted successfully.', 'success');
        setShow(false);
    }

    return (
        <Modal show={show} setShow={setShow} header={<ModalHeader title={title} />} verticallyCentered>
            {reportOptions &&
                <div className="px-4">
                    <Form action={action} onSubmit={onSubmit} onSuccess={onSuccess}>
                        <RadioGroup name="Reason">
                            {reportOptions?.ViolationOptions.map(o => (
                                <RadioField<number | undefined>
                                    id={`Reason-${o.Value}`}
                                    key={o.Value}
                                    name="Reason"
                                    value={o.Value}
                                    label={o.Label}
                                    classes="mb-3"
                                    selectedOption={reason}
                                    onChange={setReason}
                                />
                            ))}
                        </RadioGroup>

                        {reason == 7 &&
                            <TextAreaField
                                id="Message"
                                rows="4"
                                placeholder="Message..."
                                value={message}
                                onChange={setMessage}
                            />
                        }

                        <div className="text-center">
                            <Button classes="mt-3" isSubmitButton>Submit</Button>
                        </div>
                    </Form>
                </div>
            }
        </Modal>
    );
}
