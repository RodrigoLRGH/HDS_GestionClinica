import { BrowserRouter, Routes, Route } from 'react-router';
import CitasPage  from './pages/CitasPage';
import HistorialPage from './pages/HistorialPage';
import HomePage from './pages/HomePage';
import NavBar from './components/common/NavBar';

function App() {
    return (
        <BrowserRouter>
            <NavBar />
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/citas" element={<CitasPage />} />
                <Route path="/historial" element={<HistorialPage />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;