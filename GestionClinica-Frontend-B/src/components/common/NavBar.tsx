import { useNavigate, useLocation } from 'react-router';

const Navbar = () => {
    const navigate = useNavigate();
    const location = useLocation();

    return (
        <>
            <nav className="navbar navbar-expand navbar-dark bg-primary shadow-sm">
                <div className="container">
                    <span
                        className="navbar-brand fw-bold d-flex align-items-center gap-2"
                        style={{ cursor: 'pointer' }}
                        onClick={() => navigate('/')}>
                        <i className="bi bi-hospital fs-5"></i>
                        Sistema Clínico
                    </span>

                    <div className="navbar-nav gap-1">
                        <button
                            onClick={() => navigate('/')}
                            className={`btn btn-sm d-flex align-items-center gap-2 ${location.pathname === '/' ? 'btn-light text-primary' : 'btn-outline-light'}`}>
                            <i className="bi bi-house"></i> Inicio
                        </button>
                        <button
                            onClick={() => navigate('/citas')}
                            className={`btn btn-sm d-flex align-items-center gap-2 ${location.pathname === '/citas' ? 'btn-light text-primary' : 'btn-outline-light'}`}>
                            <i className="bi bi-calendar"></i> Citas
                        </button>
                        <button
                            onClick={() => navigate('/historial')}
                            className={`btn btn-sm d-flex align-items-center gap-2 ${location.pathname === '/historial' ? 'btn-light text-primary' : 'btn-outline-light'}`}>
                            <i className="bi bi-clipboard-check"></i> Historial
                        </button>
                    </div>
                </div>
            </nav>
        </>
    );
};

export default Navbar;