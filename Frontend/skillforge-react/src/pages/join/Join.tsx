import { LoginForm } from '@components/login-form/LoginForm';
import { RegisterForm } from '@components/register-form/RegisterForm';
import { Helmet } from 'react-helmet';

export function Join() {
	return (
		<div className="row">
			<Helmet>
				<title>SkillForge | Join us</title>
				<meta name="description" content="Register or Login to SkillForge" />
				<meta name="robots" content="noindex,nofollow" />
			</Helmet>
			<div className="col-12 col-lg-6">
				<LoginForm />
			</div>
			<div className="col-12 col-lg-6">
				<RegisterForm />
			</div>
		</div>
	);
}
