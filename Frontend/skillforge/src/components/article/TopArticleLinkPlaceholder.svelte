<script lang="ts">
	import Icon from "$components/icon/Icon.svelte";
</script>

<li class="list-group-item col-12">
	<div class="d-flex w-100 justify-content-between">
		<p class="h5 mb-1 w-100">
			<span class="placeholder col-10"></span>
		</p>
	</div>
	<small class="text-body-tertiary opacity-75">
		<Icon type="eye-fill" />
		<span class="placeholder col-1"></span>
        &nbsp;&nbsp;|&nbsp;&nbsp;
		<Icon type="chat-fill" />
		<span class="placeholder col-1"></span>
        &nbsp;&nbsp;|&nbsp;&nbsp;
        <span class="placeholder col-5"></span>
	</small>
</li>
