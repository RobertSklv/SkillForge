<script>
    import Form from "../../components/form/Form.svelte";
    import FormField from "../../components/form/FormField.svelte";

    let username = $state('Test');
    let password = $state();
</script>

<h1>Sign in</h1>

<Form>
    <div class="row">
        <div class="col-12 col-md-6">
            <FormField id="usernameOrEmail" type="text" label="Username or e-mail" bind:value={username} />
        </div>
        <div class="col-12 col-md-6">
            <FormField id="password" type="password" label="Password" bind:value={password} />
        </div>
    </div>
</Form>