<script lang="ts">
	import LoginForm from '$components/login-form/LoginForm.svelte';
	import RegisterForm from '$components/register-form/RegisterForm.svelte';
</script>

<div class="row">
	<div class="col-12 col-lg-6">
		<LoginForm />
	</div>
	<div class="col-12 col-lg-6">
		<RegisterForm />
	</div>
</div>

<svelte:head>
	<title>SkillForge | Join us</title>
	<meta name="description" content="Register or Login to SkillForge" />
	<meta name="robots" content="noindex,nofollow" />
</svelte:head>
