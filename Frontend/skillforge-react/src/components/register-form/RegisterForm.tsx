import { Form } from '@components/form/Form';
import { InputField } from '@components/form/input-field/InputField';
import { useState } from 'react';
import type RegisterResponse from 'skillforge-common/types/RegisterResponse';
import { remote, email as validateEmail, password as validatePassword } from 'skillforge-common/validation/rules';
import { Button } from '@components/button/Button';
import { useCurrentUser } from 'hooks/useCurrentUser';
import type UserInfo from 'skillforge-common/types/UserInfo';
import { storeAuthToken } from 'skillforge-common/auth';
import { useNavigate } from 'react-router-dom';

export function RegisterForm() {
    const [username, setUsername] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [confirmPassword, setConfirmPassword] = useState<string>('');

    const { setCurrentUser } = useCurrentUser();
    const navigate = useNavigate();

    function onSuccess(response: RegisterResponse) {
        let userInfo: UserInfo = {
            Name: username,
            Email: email,
            AvatarPath: ''
        };

        setCurrentUser(userInfo);
        storeAuthToken(response.AuthToken);

        navigate('/user/' + userInfo.Name);
    }

    return (
        <section className="mb-5">
            <h2>Sign up</h2>
            <Form<RegisterResponse>
                action="/User/Register"
                method="POST"
                onSuccess={onSuccess}
            >
                <div className="row">
                    <div className="col-12">
                        <InputField
                            id="Username"
                            type="text"
                            label="Username"
                            value={username}
                            onChange={setUsername}
                            options={{
                                required: "Please enter a valid username.",
                                validate: async (value) => await remote('/User/VerifyUniqueUsername', 'name', value) || "Username is already taken."
                            }}
                        />
                    </div>
                    <div className="col-12">
                        <InputField
                            id="Email"
                            type="email"
                            label="E-mail"
                            value={email}
                            onChange={setEmail}
                            options={{
                                required: "Please enter a valid e-mail.",
                                validate: async (value) => {
                                    if (!validateEmail(value)) return "Invalid e-mail format";
                                    if (!await remote('/User/VerifyUniqueEmail', 'email', value)) return "E-mail is already taken.";

                                    return true;
                                }
                        }} />
                    </div>
                    <div className="col-12">
                        <InputField
                            id="RegistrationPassword"
                            name="Password"
                            type="password"
                            label="Password"
                            value={password} onChange={setPassword}
                            options={{
                                required: "Please enter a valid password.",
                                validate: (value) => validatePassword(value) || "The password must be at least 8 characters long, contain at least one lowercase and uppercase letter, and at least one symbol."
                            }}
                        />
                    </div>
                    <div className="col-12">
                        <InputField
                            id="ConfirmPassword"
                            type="password"
                            label="Confirm password"
                            value={confirmPassword}
                            onChange={setConfirmPassword}
                            options={{
                                required: "Please confirm your password.",
                                validate: (value) => value === password || "Passwords don't match."
                            }}
                        />
                    </div>
                </div>

                <div className="text-center text-lg-start">
                    <Button size="lg" isSubmitButton={true}>Sign up</Button>
                </div>
            </Form>
        </section>
    );
}
