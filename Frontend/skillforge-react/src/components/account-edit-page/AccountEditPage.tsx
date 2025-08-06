'use client';

import { useToast } from '@/hooks/useToast';
import AccountInfoFormData from '@/lib/types/AccountInfoFormData';
import { ThreeColumns } from '../layout/three-columns/ThreeColumns';
import { Block } from '../block/Block';
import { Form } from '../form/Form';
import { ImageLoader } from '../form/image-loader/ImageLoader';
import { TextAreaField } from '../form/text-area-field/TextAreaField';
import { InputField } from '../form/input-field/InputField';
import { Button } from '../button/Button';
import { useState } from 'react';
import { useCurrentUser } from '@/hooks/useCurrentUser';
import { remote, email as validateEmail, password as validatePassword } from '@/lib/validation/rules';

export interface IAccountEditPageProps {
    data: AccountInfoFormData;
}

export function AccountEditPage({ data }: IAccountEditPageProps) {
    const { addToast } = useToast();
    const { currentUser } = useCurrentUser();

    const [email, setEmail] = useState<string>(data.Email);
    const [avatarImage, setAvatarImage] = useState<string | undefined>(data.AvatarImage);
    const [bio, setBio] = useState<string>(data.Bio ?? '');
    const [dateOfBirth, setDateOfBirth] = useState<string | undefined>(data.DateOfBirth);

    const [password, setPassword] = useState<string>('');
    const [confirmPassword, setConfirmPassword] = useState<string>('');
    const [currentPassword, setCurrentPassword] = useState<string>('');

    async function onInfoUpdateSuccess() {
        addToast('Account info updated successfully');
    }

    async function onPasswordUpdateSuccess() {
        addToast('Password updated successfully');

        setPassword('');
        setConfirmPassword('');
        setCurrentPassword('');
    }

    return (
        <ThreeColumns>
            <Block classes="mb-5">
                <div className="card-body">
                    <Form
                        action="/User/UpdateInfo"
                        onSuccess={onInfoUpdateSuccess}
                    >
                        <ImageLoader
                            id="AvatarImage"
                            imageAlt={`${currentUser?.Name} avatar`}
                            url={avatarImage}
                            onUrlChange={setAvatarImage}
                            imageUploadType="avatar"
                        />

                        <TextAreaField
                            id="Bio"
                            rows="5"
                            value={bio}
                            onChange={setBio}
                            maxlength="128"
                            options={{
                                maxLength: {
                                    value: 128,
                                    message: 'The biography can be maximum 128 characters long.'
                                }
                            }}
                        />

                        <InputField
                            id="Email"
                            label="E-mail"
                            type="email"
                            value={email}
                            onChange={setEmail}
                            options={{
                                required: 'Please enter a valid e-mail.',
                                maxLength: {
                                    value: 32,
                                    message: 'The e-mail can be maximum 32 characters long.'
                                },
                                validate: async (value) => {
                                    if (!validateEmail(value)) return "Invalid e-mail format";
                                    if (!await remote('/User/VerifyUniqueEmail', 'email', value)) return "E-mail is already taken.";

                                    return true;
                                }
                            }}
                        />

                        <InputField
                            id="DateOfBirth"
                            label="Date of birth"
                            type="date"
                            value={dateOfBirth}
                            onChange={setDateOfBirth}
                        />

                        <div className="text-center">
                            <Button isSubmitButton>Update info</Button>
                        </div>
                    </Form>
                </div>
            </Block>
            <Block>
                <div className="card-body">
                    <Form
                        action="/User/UpdatePassword"
                        onSuccess={onPasswordUpdateSuccess}
                    >
                        <InputField
                            id="Password"
                            type="password"
                            value={password}
                            onChange={setPassword}
                            options={{
                                required: 'Please enter a valid password.',
                                validate: value => validatePassword(value)
                                    || 'The password must be at least 8 characters long, contain at least one lowercase and uppercase letter, and at least one symbol.',
                            }}
                        />

                        <InputField
                            id="ConfirmPassword"
                            label="Confirm password"
                            type="password"
                            value={confirmPassword}
                            onChange={setConfirmPassword}
                            options={{
                                required: 'Please confirm your new password.',
                                validate: (value, fieldValues) => value === fieldValues.Password || "Passwords don't match."
                            }}
                        />

                        <InputField
                            id="CurrentPassword"
                            label="Current password"
                            type="password"
                            value={currentPassword}
                            onChange={setCurrentPassword}
                            options={{
                                required: 'Please enter your current password.'
                            }}
                        />

                        <div className="text-center">
                            <Button isSubmitButton>Update info</Button>
                        </div>
                    </Form>
                </div>
            </Block>
        </ThreeColumns>
    );
}
