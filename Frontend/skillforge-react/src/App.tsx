import { Route, Routes, useNavigate } from 'react-router-dom';
import { Join } from '@pages/join/Join';
import { useEffect } from 'react';
import { initEnv } from 'skillforge-common/env';

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
    <div className="container">
      <Routes>
        <Route path='/join' element={<Join />} />
      </Routes>
    </div>
  )
}

export default App
