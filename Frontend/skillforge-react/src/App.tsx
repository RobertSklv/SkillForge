import { Route, Routes, useNavigate } from 'react-router-dom';
import { Join } from '@pages/join/Join';
import { useEffect } from 'react';
import { initEnv, isEnvInitialized } from 'skillforge-common/env';
import { ToastContainer } from '@components/toast-container/ToastContainer';
import { Header } from '@components/header/Header';

function App() {
	const navigate = useNavigate();

	if (!isEnvInitialized()) {
		initEnv({
			baseUrl: import.meta.env.BASE_URL,
			backendUrl: import.meta.env.VITE_BACKEND_DOMAIN,
			onAuthError: () => {
				navigate('/login');
			},
			assetsRelativePath: '/src/assets/'
		});
	}

	useEffect(() => {
		document.documentElement.setAttribute('data-bs-theme', 'dark');
	}, []);

	return (
		<>
			<Header />
			<main id="main-content" className="container pb-5">
				<Routes>
					<Route path='/join' element={<Join />} />
				</Routes>
			</main>
			<ToastContainer />
		</>
	);
}

export default App;
