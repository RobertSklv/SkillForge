'use client'

import { useState } from 'react';
import type LoginResponse from '@/lib/types/LoginResponse';
import { storeAuthToken } from '@/lib/auth';
import { useCurrentUser } from '../../hooks/useCurrentUser';
import { Form } from '@/components/form/Form';
import { InputField } from '@/components/form/input-field/InputField';
import { Button } from '@/components/button/Button';
import { useToast } from '../../hooks/useToast';
import { useRouter } from 'next/navigation';


interface ILoginFormProps {
    onLogin: () => void;
}

export function LoginForm({ onLogin }: ILoginFormProps) {
    const { setCurrentUser } = useCurrentUser();
    const router = useRouter();
    const { addToast } = useToast();

    const [usernameOrEmail, setUsernameOrEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    async function onSuccess(response: LoginResponse) {
        setCurrentUser(response.CurrentUserInfo);
        storeAuthToken(response.AuthToken);
        onLogin();

        addToast('Successfully logged in');

        router.push('/user/' + response.CurrentUserInfo.Name);
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
