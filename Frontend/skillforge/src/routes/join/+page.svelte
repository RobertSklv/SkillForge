<script lang="ts">
	import LoginForm from "../../components/auth/LoginForm.svelte";
	import RegisterForm from "../../components/auth/RegisterForm.svelte";
	import Button from "../../components/button/Button.svelte";
</script>

<div class="row">
    <div class="col-12 col-lg-6">
        <LoginForm />
    </div>
    <div class="col-12 col-lg-6">
        <RegisterForm />
    </div>
</div>