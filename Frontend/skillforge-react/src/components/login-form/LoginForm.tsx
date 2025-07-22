import { useState } from 'react';
import type LoginResponse from 'skillforge-common/types/LoginResponse';
import { storeAuthToken } from 'skillforge-common/auth';
import { useNavigate } from 'react-router-dom';
import { useCurrentUser } from 'hooks/useCurrentUser';
import { Form } from '@components/form/Form';
import { InputField } from '@components/form/input-field/InputField';
import { Button } from '@components/button/Button';
import { useToast } from 'hooks/useToast';

export function LoginForm() {
    const { setCurrentUser } = useCurrentUser();
    const navigate = useNavigate();
    const { addToast } = useToast();

    const [usernameOrEmail, setUsernameOrEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    async function onSuccess(response: LoginResponse) {
        setCurrentUser(response.CurrentUserInfo);
        storeAuthToken(response.AuthToken);

        addToast('Successfully logged in');

        navigate('/user/' + response.CurrentUserInfo.Name);
    }

    return (
        <section className="mb-5">
            <h2>Sign in</h2>
            <Form
                action="/User/Auth"
                method="POST"
                onSuccess={onSuccess}
            >
                <div className="row">
                    <div className="col-12">
                        <InputField
                            id="UsernameOrEmail"
                            type="text"
                            label="Username or e-mail"
                            value={usernameOrEmail}
                            onChange={setUsernameOrEmail}
                            options={{
                                required: 'Please enter a valid username or e-mail address.'
                            }}
                        />
                    </div>
                    <div className="col-12">
                        <InputField
                            id="Password"
                            type="password"
                            label="Password"
                            value={password}
                            onChange={setPassword}
                            options={{
                                required: 'Please enter your password.'
                            }}
                        />
                    </div>
                </div>

                <div className="text-center text-lg-start">
                    <Button size="lg" isSubmitButton={true}>Sign in</Button>
                </div>
            </Form>
        </section>
    );
}
