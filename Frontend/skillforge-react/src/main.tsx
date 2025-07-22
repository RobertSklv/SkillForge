import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import 'bootswatch/dist/vapor/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.min.css';
import App from './App.tsx';
import { BrowserRouter as Router } from 'react-router-dom';

createRoot(document.getElementById('root')!).render(
  <Router>
    <StrictMode>
      <App />
    </StrictMode>
  </Router>,
)
