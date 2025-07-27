import { LoginForm } from "@/components/login-form/LoginForm";
import { RegisterForm } from "@/components/register-form/RegisterForm";
import { Metadata } from "next";

export const metadata: Metadata = {
    title: 'SkillForge | Join us',
    description: 'Register or Login to SkillForge',
    robots: 'noindex,nofollow'
};

export default function Join() {
	return (
		<div className="row">
			<div className="col-12 col-lg-6">
				<LoginForm />
			</div>
			<div className="col-12 col-lg-6">
				<RegisterForm />
			</div>
		</div>
	);
}