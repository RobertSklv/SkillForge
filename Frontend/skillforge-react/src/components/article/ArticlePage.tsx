'use client';

import { useState, type ReactNode } from 'react';
import type ArticlePageModel from '@/lib/types/ArticlePageModel';
import { Block } from '@/components/block/Block';
import { AuthorBox } from '@/components/author-box/AuthorBox';
import { Icon } from '@/components/icon/Icon';
import { getBackendUrl } from '@/lib/util';
import { useCurrentUser } from '../../hooks/useCurrentUser';
import { Dropdown } from '@/components/dropdown/Dropdown';
import { DropdownItem } from '@/components/dropdown-item/DropdownItem';
import { DropdownDivider } from '@/components/dropdown-divider/DropdownDivider';
import type CommentModel from '@/lib/types/CommentModel';
import { AnimatePresence, motion } from 'framer-motion';
import { RateButtons } from '@/components/rate-buttons/RateButtons';
import { Comment } from '@/components/comment/Comment';
import { ArticleContext } from '@/context/ArticleContext';
import './_article-page.scss';
import { Form } from '../form/Form';
import { TextEditor } from '../form/text-editor/TextEditor';
import { Button } from '../button/Button';
import { LoginCta } from '../login-cta/LoginCta';
import { ReportModal } from '../report-modal/ReportModal';

export interface IArticleProps {
    data: ArticlePageModel;
}

export function Article({ data }: IArticleProps) {
    const { currentUser } = useCurrentUser();

    const [comments, setComments] = useState<CommentModel[]>(data.Comments);
    const [showReportModal, setShowReportModal] = useState<boolean>(false);

    const [commentFormContent, setCommentFormContent] = useState<string | undefined>();

    async function addComment(comment: CommentModel) {
        if (!currentUser) {
            throw Error('User not logged in');
        }

        setCommentFormContent('');
        setComments([...comments, comment]);
    }

    function onCommentSubmit(commentData: any) {
        commentData.ArticleId = data.ArticleId;

        return commentData;
    }

    function deleteComment(id: number) {
        setComments(comments.filter((c) => c.CommentId != id));
    }

    function header(): ReactNode {
        return (
            <div className="row mb-3 pt-2">
                <div className="col">
                    <AuthorBox
                        name={data.Author.Link.Name}
                        avatarImage={data.Author.Link.AvatarImage}
                        date={data.DatePublished}
                        indent={false}
                    />
                </div>
                {currentUser &&
                    <div className="col-3 text-end">
                        <Dropdown menuClass="dropdown-menu-end dropdown-menu-xl-start" buttonSnippet={<Icon type="three-dots-vertical" />} hideChevron>
                            {currentUser.Name == data.Author.Link.Name && (
                                <>
                                    <DropdownItem href={`/article/${data.ArticleId}/edit`}>
                                        <Icon type="pencil-square" classes='me-2' />
                                        Edit
                                    </DropdownItem>
                                    <DropdownDivider />
                                </>
                            )}
                            <DropdownItem type="button" onClick={() => (setShowReportModal(true))}>
                                <Icon type="exclamation-triangle" classes='me-2' />
                                Report
                            </DropdownItem>
                        </Dropdown>
                    </div>
                }
            </div>
        );
    }

    function footer(): ReactNode {
        return (
            <div className="row">
                <div className="col-3 fs-5 d-flex align-items-center">
                    <Icon type="eye" classes="me-1" />
                    <small className="text-muted">{data.Views}</small>
                </div>
                <div className="col-9">
                    <RateButtons
                        data={data.RatingData}
                        subjectId={data.ArticleId}
                        type="article"
                        readonly={!currentUser}
                    />
                </div>
            </div>
        );
    }

    return (
        <>
            <div className="d-flex flex-column gap-5">
                <Block header={header()} footer={footer()}>
                    {data.CoverImage &&
                        <div
                            className="cover-image"
                            style={{
                                backgroundImage: `url('${getBackendUrl(data.CoverImage)}')`
                            }}
                        ></div>
                    }

                    <article className="card-body">
                        {data.Tags.map(tag => {
                            return <a href={`/tag/${tag}`} className="me-2" key={tag}>#{tag}</a>;
                        })}
                        <h1 className="card-title mb-4">{data.Title}</h1>
                        <div className="card-text text-break rich-text-content" dangerouslySetInnerHTML={{ __html: data.Content }}></div>
                    </article>
                </Block>

                <div className="d-flex flex-column gap-3">
                    <AnimatePresence>
                        <ArticleContext.Provider value={{
                            deleteComment
                        }}>
                            {comments.map((comment) => {
                                return (
                                    <motion.div
                                        key={`comment-${comment.CommentId}`}
                                        initial={{ opacity: 0 }}
                                        animate={{ opacity: 1 }}
                                        exit={{ opacity: 0 }}
                                        transition={{ duration: 0.2 }}
                                        layout
                                    >
                                        <Comment data={comment} />
                                    </motion.div>
                                );
                            })}
                        </ArticleContext.Provider>
                    </AnimatePresence>
                </div>

                {currentUser &&
                    <Block>
                        <div className="card-body">
                            <Form<CommentModel>
                                action="/Comment/Upsert"
                                onSubmit={onCommentSubmit}
                                onSuccess={addComment}
                            >
                                <TextEditor
                                    id="CommentContent"
                                    name="Content"
                                    label={null}
                                    height={200}
                                    content={commentFormContent}
                                    imageUploadType="comment"
                                />

                                <div className="text-center">
                                    <Button isSubmitButton={true}>Comment</Button>
                                </div>
                            </Form>
                        </div>
                    </Block>
                }

                <LoginCta ctaText="Log in" description="to comment and rate content." inline={true} />
            </div>

            <ReportModal
                title="Report article"
                entityId={data.ArticleId}
                action="/ArticleReport/Create"
                show={showReportModal}
                setShow={setShowReportModal}
            />
        </>
    );
}
