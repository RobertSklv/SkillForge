import { Route, Routes, useNavigate } from 'react-router-dom';
import { Home } from '@pages/home/Home';
import { Join } from '@pages/join/Join';
import { useEffect } from 'react';
import { initEnv } from 'skillforge-common/env';
import { ToastContainer } from '@components/toast-container/ToastContainer';

function App() {
	const navigate = useNavigate();

	useEffect(() => {
		document.documentElement.setAttribute('data-bs-theme', 'dark');

		initEnv({
			baseUrl: import.meta.env.BASE_URL,
			backendUrl: import.meta.env.VITE_BACKEND_DOMAIN,
			onAuthError: () => {
				navigate('/login');
			}
		});
	}, []);

	return (
		<>
			<div className="container">
				<Routes>
					<Route path='/' element={<Home />} />
					<Route path='/join' element={<Join />} />
				</Routes>
			</div>
			<ToastContainer />
		</>
	);
}

export default App;
