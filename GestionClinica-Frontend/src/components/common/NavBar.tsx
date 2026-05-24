import { useNavigate, useLocation } from 'react-router';
import { Hospital, Calendar, ClipboardList, Home } from 'lucide-react';
import { Button } from '../../components/ui/button';

const Navbar = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const activo = (ruta: string) =>
        location.pathname === ruta
            ? 'bg-blue-700 text-white px-4 py-2 rounded flex items-center gap-2'
            : 'text-white px-4 py-2 rounded hover:bg-blue-700 flex items-center gap-2';

    return (
        <nav className="bg-blue-600 shadow-md">
            <div className="max-w-6xl mx-auto px-4 py-3 flex justify-between items-center">
                <div
                    className="text-white font-bold text-xl cursor-pointer flex items-center gap-2"
                    onClick={() => navigate('/')}>
                    <Hospital size={24} />
                    Sistema Clínico
                </div>
                <div className="flex gap-2">
                    <Button variant="ghost" onClick={() => navigate('/')} className={activo('/')}>
                        <Home size={18} /> Inicio
                    </Button>
                    <Button variant="ghost" onClick={() => navigate('/citas')} className={activo('/citas')}>
                        <Calendar size={18} /> Citas
                    </Button>
                    <Button variant="ghost" onClick={() => navigate('/historial')} className={activo('/historial')}>
                        <ClipboardList size={18} /> Historial
                    </Button>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;