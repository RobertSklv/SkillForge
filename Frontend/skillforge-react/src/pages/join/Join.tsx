import { LoginForm } from '@components/login-form/LoginForm';
import { RegisterForm } from '@components/register-form/RegisterForm';

export function Join () {
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
