<script lang="ts">
	import AuthorBoxPlaceholder from "$components/user/AuthorBoxPlaceholder.svelte";
</script>

<AuthorBoxPlaceholder size="small" />
<div class="mx-5 p-3">
    <div class="content-wrapper rounded-3 p-3 placeholder col-12"></div>
</div>

<style>
    .content-wrapper {
        background-color: rgba(111, 66, 193, 50%);
        height: 72px;
    }
</style>