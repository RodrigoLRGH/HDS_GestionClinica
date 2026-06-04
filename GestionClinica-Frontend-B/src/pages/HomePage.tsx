import { useNavigate } from 'react-router';

const HomePage = () => {
    const navigate = useNavigate();

    return (
        <>
            <div className="min-vh-100 bg-light d-flex flex-column align-items-center justify-content-center px-3">
                <div className="text-center mb-5">
                    <i className="bi bi-hospital display-3 text-primary d-block mb-3"></i>
                    <h1 className="fw-bold text-dark mb-2">Sistema de Gestión Clínica</h1>
                    <p className="text-muted fs-5">
                        Administra citas médicas e historial clínico de pacientes
                    </p>
                </div>

                <div className="row g-4 w-100 justify-content-center">
                    <div className="col-12 col-md-5">
                        <div
                            onClick={() => navigate('/citas')}
                            className="card shadow-sm rounded-3 p-4 h-100 border-0"
                            style={{ cursor: 'pointer', transition: 'transform 0.2s, box-shadow 0.2s' }}
                            onMouseEnter={e => {
                                (e.currentTarget as HTMLElement).style.transform = 'scale(1.03)';
                                (e.currentTarget as HTMLElement).style.boxShadow = '0 .5rem 1.5rem rgba(0,0,0,.15)';
                            }}
                            onMouseLeave={e => {
                                (e.currentTarget as HTMLElement).style.transform = 'scale(1)';
                                (e.currentTarget as HTMLElement).style.boxShadow = '';
                            }}>
                            <i className="bi bi-calendar fs-2 text-primary mb-3 d-block"></i>
                            <h5 className="fw-bold mb-1">Citas Médicas</h5>
                            <p className="text-muted small">
                                Registra, consulta, edita y cancela citas médicas. Filtra por paciente, fecha o estado.
                            </p>
                            <span className="text-primary fw-semibold small">
                                Ir a Citas <i className="bi bi-arrow-right"></i>
                            </span>
                        </div>
                    </div>

                    <div className="col-12 col-md-5">
                        <div
                            onClick={() => navigate('/historial')}
                            className="card shadow-sm rounded-3 p-4 h-100 border-0"
                            style={{ cursor: 'pointer', transition: 'transform 0.2s, box-shadow 0.2s' }}
                            onMouseEnter={e => {
                                (e.currentTarget as HTMLElement).style.transform = 'scale(1.03)';
                                (e.currentTarget as HTMLElement).style.boxShadow = '0 .5rem 1.5rem rgba(0,0,0,.15)';
                            }}
                            onMouseLeave={e => {
                                (e.currentTarget as HTMLElement).style.transform = 'scale(1)';
                                (e.currentTarget as HTMLElement).style.boxShadow = '';
                            }}>
                            <i className="bi bi-clipboard-check fs-2 text-success mb-3 d-block"></i>
                            <h5 className="fw-bold mb-1">Historial Clínico</h5>
                            <p className="text-muted small">
                                Consulta el historial completo de evoluciones por paciente y agrega nuevos registros.
                            </p>
                            <span className="text-success fw-semibold small">
                                Ir a Historial <i className="bi bi-arrow-right"></i>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default HomePage;