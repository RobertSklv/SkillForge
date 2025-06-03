<script lang="ts">
	import Button from "$components/button/Button.svelte";
	import Comment from "$components/comment/Comment.svelte";
	import Dropdown from "$components/dropdown/Dropdown.svelte";
	import DropdownDivider from "$components/dropdown/DropdownDivider.svelte";
	import DropdownItem from "$components/dropdown/DropdownItem.svelte";
	import Form from "$components/form/Form.svelte";
	import TextEditor from "$components/form/TextEditor.svelte";
	import Block from "$components/layout/Block.svelte";
	import LoginCta from "$components/login-cta/LoginCta.svelte";
	import RateButtons from "$components/rating/RateButtons.svelte";
	import AuthorBox from "$components/user/AuthorBox.svelte";
	import { PUBLIC_BACKEND_DOMAIN } from "$env/static/public";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import type ArticlePageModel from "$lib/types/ArticlePageModel";
	import type CommentModel from "$lib/types/CommentModel";
	import moment from "moment";
	import { writable } from "svelte/store";

    interface Props {
        data: ArticlePageModel
    }

    interface CommentFormData {
        ArticleId: number,
        Content: string
    }

    let {
        data
    }: Props = $props();

    let commentFormData = writable<CommentFormData>({
        ArticleId: data.ArticleId,
        Content: ''
    });

    let comments = $state<CommentModel[]>(data.Comments);

    async function addComment() {
        if (!$currentUserStore) {
            throw Error('User not logged in');
        }

        let comment: CommentModel = {
            CommentId: 0,
            Content: $commentFormData.Content,
            User: {
                Name: $currentUserStore.Name,
                AvatarImage: $currentUserStore.AvatarPath
            },
            DateWritten: new Date().toDateString(),
            RatingData: {
                ThumbsUp: 0,
                ThumbsDown: 0,
                UserRating: 0,
            }
        }

        comments.push(comment);
    }
</script>

<div class="d-flex flex-column gap-5">
    <Block>
        {#snippet header()}
            <div class="row mb-3">
                <div class="col">
	                <AuthorBox name={data.Author.Name} date={data.DatePublished} indent={false} />
                </div>
                <div class="col text-end">
                    <Dropdown>
                        {#snippet buttonSnippet()}
                            <i class="bi bi-three-dots-vertical"></i>
                        {/snippet}
                        {#if $currentUserStore && $currentUserStore.Name == data.Author.Name}
                            <DropdownItem href="/article/{data.ArticleId}/edit">
                                <i class="bi bi-pencil-square"></i>
                                Edit
                            </DropdownItem>
                            <DropdownDivider />
                        {/if}
                        <DropdownItem href="/">
                            <i class="bi bi-exclamation-triangle"></i>
                            Report
                        </DropdownItem>
                    </Dropdown>
                </div>
            </div>
        {/snippet}

        {#if data.CoverImage}
            <div class="cover-image" style="background-image: url('{PUBLIC_BACKEND_DOMAIN + data.CoverImage}')"></div>
        {/if}
        
        <div class="card-body">
            {#each data.Tags as tag}
                <a href="/tag/{tag}" class="me-2">#{tag}</a>
            {/each}
            <h1 class="card-title mb-4">{data.Title}</h1>
            <div class="card-text text-break">
                {@html data.Content}
            </div>
        </div>

        {#snippet footer()}
            <div class="row">
                <div class="col fs-5 d-flex align-items-center">
                    <i class="bi bi-eye me-1"></i>
                    <small class="text-muted">{data.Views}</small>
                </div>
                <div class="col">
                    <RateButtons data={data.RatingData} subjectId={data.ArticleId} type="article" readonly={!$currentUserStore} />
                </div>
            </div>
        {/snippet}
    </Block>

    <div class="d-flex flex-column gap-3">
        {#each comments as comment}
            <Comment data={comment} />
        {/each}
    </div>

    {#if $currentUserStore}
        <Block>
            <div class="card-body">
                <Form action="/Comment/Add" formData={$commentFormData} onSuccess={addComment}>
                    <TextEditor id="Content" label={null} height={200} bind:content={$commentFormData.Content} imageUploadType="comment" />

                    <div class="text-center">
                        <Button isSubmitButton={true}>Comment</Button>
                    </div>
                </Form>
            </div>
        </Block>
    {/if}

    <LoginCta ctaText="Log in" description="to comment and rate content." inline={true} />
</div>

<style>
    .cover-image {
        height: 300px;
        box-shadow: 0px 0px 99px 32px rgba(0,0,0,1) inset;
        -webkit-box-shadow: 0px 0px 99px 32px rgba(0,0,0,1) inset;
        -moz-box-shadow: 0px 0px 99px 32px rgba(0,0,0,1) inset;
        background-repeat: no-repeat;
        background-size: cover;
    }
</style>