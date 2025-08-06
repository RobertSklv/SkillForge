import { LoginForm } from "@/components/login-form/LoginForm";
import { RegisterForm } from "@/components/register-form/RegisterForm";
import { Metadata } from "next";
import { revalidateTag } from "next/cache";

export const metadata: Metadata = {
    title: 'SkillForge | Join us',
    description: 'Register or Login to SkillForge',
    robots: 'noindex,nofollow'
};

async function revalidateAuth() {
	'use server'

	revalidateTag('auth');
}

export default function Join() {
	return (
		<div className="row">
			<div className="col-12 col-lg-6">
				<LoginForm onLogin={revalidateAuth} />
			</div>
			<div className="col-12 col-lg-6">
				<RegisterForm />
			</div>
		</div>
	);
}