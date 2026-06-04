import { type Evolucion } from '../../types/Evolucion/Evolucion';

interface Props {
    evoluciones: Evolucion[];
}

const ListaEvoluciones = ({ evoluciones }: Props) => {
    if (evoluciones.length === 0) {
        return (
            <div className="text-center text-muted py-5">
                <i className="bi bi-clipboard display-1 opacity-25 d-block mb-2"></i>
                <p className="fs-5">No hay evoluciones registradas para este paciente.</p>
            </div>
        );
    }

    return (
        <>
            <div className="d-flex flex-column gap-3">
                {evoluciones.map(evolucion => (
                    <div key={evolucion.id} className="card shadow-sm rounded-3 overflow-hidden">
                        <div className="card-header bg-success bg-opacity-10 d-flex justify-content-between align-items-center">
                            <div className="d-flex align-items-center gap-2 text-muted">
                                <i className="bi bi-calendar"></i>
                                <span className="small fw-medium">
                                    {new Date(evolucion.fecha).toLocaleDateString('es-MX', {
                                        year: 'numeric',
                                        month: 'long',
                                        day: 'numeric'
                                    })}
                                </span>
                            </div>
                            <div className="d-flex align-items-center gap-2 text-muted">
                                <i className="bi bi-heart-pulse"></i>
                                <span className="small fw-medium">
                                    {evolucion.doctor?.nombreCompleto ?? '—'}
                                </span>
                            </div>
                        </div>

                        <div className="card-body">
                            <div className="row g-3">
                                <div className="col-12 col-md-6">
                                    <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3 h-100">
                                        <i className="bi bi-file-text text-secondary mt-1 flex-shrink-0"></i>
                                        <div>
                                            <p className="text-uppercase text-muted small fw-semibold mb-1">Diagnóstico</p>
                                            <p className="text-muted small mb-0">{evolucion.diagnostico}</p>
                                        </div>
                                    </div>
                                </div>

                                <div className="col-12 col-md-6">
                                    <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3 h-100">
                                        <i className="bi bi-capsule text-secondary mt-1 flex-shrink-0"></i>
                                        <div>
                                            <p className="text-uppercase text-muted small fw-semibold mb-1">Tratamiento</p>
                                            <p className="text-muted small mb-0">{evolucion.tratamiento}</p>
                                        </div>
                                    </div>
                                </div>

                                {evolucion.notas && (
                                    <div className="col-12">
                                        <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3">
                                            <i className="bi bi-sticky text-secondary mt-1 flex-shrink-0"></i>
                                            <div>
                                                <p className="text-uppercase text-muted small fw-semibold mb-1">Notas</p>
                                                <p className="text-muted small mb-0">{evolucion.notas}</p>
                                            </div>
                                        </div>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </>
    );
};

export default ListaEvoluciones;