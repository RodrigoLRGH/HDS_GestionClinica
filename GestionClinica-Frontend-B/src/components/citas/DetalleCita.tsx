import { type Cita } from '../../types/Cita/Cita';
import { EstadoCita } from '../../types/Enumeraciones/EstadoCita';

interface Props {
    cita: Cita;
    onVolver: () => void;
    onEditar: () => void;
}

const textoEstado: Record<number, string> = {
    1: 'Pendiente',
    2: 'Confirmada',
    3: 'Cancelada',
    4: 'Completada',
    5: 'No Asistió'
};

const DetalleCita = ({ cita, onVolver, onEditar }: Props) => {
    const badgeEstado = (estado: EstadoCita) => {
        const clases: Record<number, string> = {
            1: 'bg-warning text-dark',
            2: 'bg-primary',
            3: 'bg-danger',
            4: 'bg-success',
            5: 'bg-secondary',
        };
        return `badge rounded-pill ${clases[estado] ?? 'bg-secondary'}`;
    };

    return (
        <>
            <div className="container py-4" style={{ maxWidth: '720px' }}>
                <div className="d-flex align-items-center gap-2 mb-4">
                    <i className="bi bi-file-text fs-3 text-primary"></i>
                    <h2 className="fw-bold mb-0">Detalle de Cita</h2>
                </div>

                <div className="card shadow-sm rounded-3 overflow-hidden">
                    <div className="card-header bg-primary text-white">
                        <span className="fw-medium">Cita #{cita.id}</span>
                    </div>

                    <div className="card-body p-4">
                        <div className="row g-3">

                            <div className="col-12 col-md-6">
                                <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3 h-100">
                                    <i className="bi bi-person fs-5 text-secondary mt-1 flex-shrink-0"></i>
                                    <div>
                                        <p className="text-uppercase text-muted small fw-semibold mb-1">Paciente</p>
                                        <p className="fw-semibold mb-0">{cita.paciente?.nombreCompleto ?? '—'}</p>
                                    </div>
                                </div>
                            </div>

                            <div className="col-12 col-md-6">
                                <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3 h-100">
                                    <i className="bi bi-heart-pulse fs-5 text-secondary mt-1 flex-shrink-0"></i>
                                    <div>
                                        <p className="text-uppercase text-muted small fw-semibold mb-1">Doctor</p>
                                        <p className="fw-semibold mb-0">{cita.doctor?.nombreCompleto ?? '—'}</p>
                                    </div>
                                </div>
                            </div>

                            <div className="col-12 col-md-6">
                                <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3 h-100">
                                    <i className="bi bi-clock fs-5 text-secondary mt-1 flex-shrink-0"></i>
                                    <div>
                                        <p className="text-uppercase text-muted small fw-semibold mb-1">Fecha y Hora</p>
                                        <p className="fw-semibold mb-0">
                                            {new Date(cita.fechaHora).toLocaleString('es-MX')}
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <div className="col-12 col-md-6">
                                <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3 h-100">
                                    <i className="bi bi-info-circle fs-5 text-secondary mt-1 flex-shrink-0"></i>
                                    <div>
                                        <p className="text-uppercase text-muted small fw-semibold mb-1">Estado</p>
                                        <span className={badgeEstado(cita.estado)}>
                                            {textoEstado[cita.estado]}
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div className="col-12">
                                <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3">
                                    <i className="bi bi-file-text fs-5 text-secondary mt-1 flex-shrink-0"></i>
                                    <div>
                                        <p className="text-uppercase text-muted small fw-semibold mb-1">Motivo</p>
                                        <p className="fw-semibold mb-0">{cita.motivo}</p>
                                    </div>
                                </div>
                            </div>

                            <div className="col-12">
                                <div className="d-flex align-items-start gap-2 bg-light rounded-3 p-3">
                                    <i className="bi bi-sticky fs-5 text-secondary mt-1 flex-shrink-0"></i>
                                    <div>
                                        <p className="text-uppercase text-muted small fw-semibold mb-1">Notas</p>
                                        <p className="text-muted mb-0">{cita.notas ?? '—'}</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="d-flex justify-content-end gap-2 mt-4 pt-3 border-top">
                            <button
                                onClick={onVolver}
                                className="btn btn-secondary d-flex align-items-center gap-2">
                                <i className="bi bi-arrow-left"></i> Volver
                            </button>
                            {cita.estado !== EstadoCita.Cancelada && (
                                <button
                                    onClick={onEditar}
                                    className="btn btn-warning d-flex align-items-center gap-2">
                                    <i className="bi bi-pencil"></i> Editar
                                </button>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default DetalleCita;