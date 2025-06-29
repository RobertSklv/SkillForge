<script lang="ts">
	import CommentLimitedPlaceholder from '$components/comment/CommentLimitedPlaceholder.svelte';
	import Icon from '$components/icon/Icon.svelte';
	import Block from '$components/layout/Block.svelte';
	import LinkPlaceholder from '$components/link/LinkPlaceholder.svelte';
	import TagLinkPlaceholder from '$components/link/TagLinkPlaceholder.svelte';
	import RateButtonsPlaceholder from '$components/rating/RateButtonsPlaceholder.svelte';
	import AuthorBoxPlaceholder from '$components/user/AuthorBoxPlaceholder.svelte';
</script>

<Block mod="placeholder-glow">
	<div class="placeholder card-img-top rounded-top-3 object-fit-cover" style="height: 250px"></div>
	<AuthorBoxPlaceholder mod="mt-3" />
	<div class="card-body mx-5">
		<div class="d-flex justify-content-between align-items-start">
			<div class="h2 card-title fw-bold w-100">
                <span class="placeholder col-5"></span>
            </div>
		</div>
		<div class="d-flex justify-content-between">
			<div class="col">
				<div class="tags d-flex gap-1">
                    <TagLinkPlaceholder size="small" background="fill"/>
				</div>
			</div>
			<div>
				<div class="d-flex align-items-center">
					<RateButtonsPlaceholder size="small" />
                    <div class="text-primary d-flex align-items-center px-2">
                        <Icon type="chat" mod="fs-6" />
                        <span class="placeholder placeholder-sm col-3 ms-1 h-auto"></span>
                    </div>
				</div>
			</div>
		</div>
	</div>
	<div>
        <CommentLimitedPlaceholder />
	</div>
    <div class="text-center mt-2 mb-4">
        <LinkPlaceholder background="fill" />
    </div>
</Block>