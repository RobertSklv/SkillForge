import { AuthorBox } from '@/components/author-box/AuthorBox';
import { Block } from '@/components/block/Block';
import { Button } from '@/components/button/Button';
import { DropdownDivider } from '@/components/dropdown-divider/DropdownDivider';
import { DropdownItem } from '@/components/dropdown-item/DropdownItem';
import { Dropdown } from '@/components/dropdown/Dropdown';
import { Form } from '@/components/form/Form';
import { Icon } from '@/components/icon/Icon';
import { RateButtons } from '@/components/rate-buttons/RateButtons';
import { useArticleContext } from '../../context/ArticleContext';
import { useCurrentUser } from '../../hooks/useCurrentUser';
import { useToast } from '../../hooks/useToast';
import { useState } from 'react';
import { deleteComment } from '@/lib/api/client';
import type CommentModel from '@/lib/types/CommentModel';

export interface ICommentProps {
    data: CommentModel;
}

export function Comment({ data }: ICommentProps) {
    const { currentUser } = useCurrentUser();
    const { addToast } = useToast();

    const [showReportModal, setShowReportModal] = useState<boolean>(false);
    const [editMode, setEditMode] = useState<boolean>(false);
    const [editCommentContent, setEditCommentContent] = useState<string>('');

    const { deleteComment: deleteCommentFromArticleContext } = useArticleContext();

	function onEditSuccess(response: CommentModel) {
        setEditMode(false);
		data.Content = response.Content;
		setEditCommentContent(response.Content);
		data.DateWritten = response.DateWritten;
		data.DateEdited = response.DateEdited;

		addToast('Comment edited successfully');
	}

	function cancelEdit() {
        setEditMode(false);
		setEditCommentContent(data.Content);
	}

	async function onDelete() {
		let commentId = data.CommentId;
		await deleteComment(commentId).then((r) => {
			deleteCommentFromArticleContext(commentId);
			addToast('Comment deleted successfully');
		});
	}

    function header() {
        return (
            
                <div className="row mb-3 pt-2">
                    <div className="col">
                        <AuthorBox
                            name={data.User.Name}
                            avatarImage={data.User.AvatarImage}
                            date={data.DateWritten}
                            editedDate={data.DateEdited}
                            size="small"
                            indent={false}
                        />
                    </div>
                    {currentUser &&
                    <div className="col-3 text-end">
                        <Dropdown menuClass="dropdown-menu-end dropdown-menu-xl-start" buttonSnippet={<Icon type="three-dots-vertical" />} hideChevron>
                            {(currentUser.Name == data.User.Name) && (
                                <>
                                    <DropdownItem type="button" onClick={() => (setEditMode(true))}>
                                        <Icon type="pencil-square" />
                                        Edit
                                    </DropdownItem>
                                    <DropdownItem type="button" onClick={onDelete}>
                                        <Icon type="trash" />
                                        Delete
                                    </DropdownItem>
                                    <DropdownDivider />
                                </>
                            )}
                            <DropdownItem type="button" onClick={() => (setShowReportModal(true))}>
                                <Icon type="exclamation-triangle" />
                                Report
                            </DropdownItem>
                        </Dropdown>
                    </div>
                    }
                </div>
        )
    }

    function footer() {
        return (
                <RateButtons
                    data={data.RatingData}
                    subjectId={data.CommentId}
                    type="comment"
                    readonly={!currentUser}
                />
        )
    }

    return (
        <>
            <Block header={header()} footer={footer()}>
                {editMode ? (
                <div className="card-body">
                    {/* <Form action="/Comment/Upsert" onSuccess={onEditSuccess}>
                        <TextEditor
                            id="CommentEditContent-{data.CommentId}"
                            name="Content"
                            label={null}
                            height={200}
                            bind:content={$editFormData.Content}
                            imageUploadType="comment"
                        />

                        <div className="text-center">
                            <Button color="secondary" onClick={cancelEdit} classes="me-3">Cancel</Button>
                            <Button isSubmitButton={true}>Edit</Button>
                        </div>
                    </Form> */}
                </div>
                ) : (
                    <div className="card-body">
                        <div className="text-break rich-text-content" dangerouslySetInnerHTML={{ __html: data.Content }}></div>
                    </div>
                )}
            </Block>

            {/* <ReportModal entityId={data.CommentId} action="/CommentReport/Create" bind:show={showReportModal} /> */}
        </>
    );
}
