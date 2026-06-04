import { useState } from 'react';
import { type Cita } from '../../types/Cita/Cita';
import { EstadoCita } from '../../types/Enumeraciones/EstadoCita';
import LoadingSpinner from '../common/LoadingSpinner';
import ErrorAlert from '../common/ErrorAlert';
import ModalConfirmacion from '../common/ModalConfirmacion';

const textoEstado: Record<number, string> = {
    1: 'Pendiente',
    2: 'Confirmada',
    3: 'Cancelada',
    4: 'Completada',
    5: 'No Asistió'
};

const badgeEstado = (estado: number) => {
    const clases: Record<number, string> = {
        1: 'bg-warning text-dark',
        2: 'bg-primary',
        3: 'bg-danger',
        4: 'bg-success',
        5: 'bg-secondary',
    };
    return `badge rounded-pill ${clases[estado] ?? 'bg-secondary'}`;
};

const ListaCitas = ({ citas, cargando, error, onCancelar, onNueva, onEditar, onDetalle }: {
    citas: Cita[];
    cargando: boolean;
    error: string | null;
    onCancelar: (id: number) => Promise<boolean>;
    onNueva: () => void;
    onEditar: (cita: Cita) => void;
    onDetalle: (cita: Cita) => void;
}) => {
    const [filtroPaciente, setFiltroPaciente] = useState('');
    const [filtroEstado, setFiltroEstado] = useState<number | ''>('');
    const [filtroFecha, setFiltroFecha] = useState('');
    const [citaACancelar, setCitaACancelar] = useState<number | null>(null);

    const citasFiltradas = citas.filter(c => {
        const nombrePaciente = c.paciente?.nombreCompleto?.toLowerCase() ?? '';
        const coincidePaciente = nombrePaciente.includes(filtroPaciente.toLowerCase());
        const coincideEstado = filtroEstado === '' || c.estado === filtroEstado;
        const coincideFecha = filtroFecha === '' || c.fechaHora.startsWith(filtroFecha);
        return coincidePaciente && coincideEstado && coincideFecha;
    });

    const handleCancelar = async () => {
        if (citaACancelar !== null) {
            await onCancelar(citaACancelar);
            setCitaACancelar(null);
        }
    };

    if (cargando) return <LoadingSpinner />;

    return (
        <>
            <div className="container-fluid p-4">
                <div className="d-flex justify-content-between align-items-center mb-4">
                    <div className="d-flex align-items-center gap-2">
                        <i className="bi bi-calendar fs-3 text-primary"></i>
                        <h2 className="fw-bold mb-0">Citas Médicas</h2>
                    </div>
                    <button
                        onClick={onNueva}
                        className="btn btn-primary d-flex align-items-center gap-2">
                        <i className="bi bi-plus-lg"></i> Nueva Cita
                    </button>
                </div>

                {error && <ErrorAlert mensaje={error} />}

                <div className="card shadow-sm rounded-3 mb-4">
                    <div className="card-body p-3">
                        <div className="d-flex align-items-center gap-2 mb-3 text-muted">
                            <i className="bi bi-funnel"></i>
                            <span className="small fw-medium">Filtros</span>
                        </div>
                        <div className="row g-3">
                            <div className="col-12 col-md-4">
                                <div className="input-group">
                                    <span className="input-group-text bg-white border-end-0">
                                        <i className="bi bi-search text-muted"></i>
                                    </span>
                                    <input
                                        type="text"
                                        placeholder="Buscar por paciente..."
                                        className="form-control border-start-0"
                                        value={filtroPaciente}
                                        onChange={e => setFiltroPaciente(e.target.value)}
                                    />
                                </div>
                            </div>
                            <div className="col-12 col-md-4">
                                <select
                                    className="form-select"
                                    value={filtroEstado}
                                    onChange={e => setFiltroEstado(e.target.value === '' ? '' : Number(e.target.value))}>
                                    <option value="">Todos los estados</option>
                                    <option value={1}>Pendiente</option>
                                    <option value={2}>Confirmada</option>
                                    <option value={3}>Cancelada</option>
                                    <option value={4}>Completada</option>
                                    <option value={5}>No Asistió</option>
                                </select>
                            </div>
                            <div className="col-12 col-md-4">
                                <div className="input-group">
                                    <span className="input-group-text bg-white border-end-0">
                                        <i className="bi bi-calendar text-muted"></i>
                                    </span>
                                    <input
                                        type="date"
                                        className="form-control border-start-0"
                                        value={filtroFecha}
                                        onChange={e => setFiltroFecha(e.target.value)}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                {citasFiltradas.length === 0 ? (
                    <div className="text-center text-muted py-5">
                        <i className="bi bi-calendar display-1 opacity-25 d-block mb-2"></i>
                        <p>No hay citas registradas.</p>
                    </div>
                ) : (
                    <div className="card shadow-sm rounded-3 overflow-hidden">
                        <div className="table-responsive">
                            <table className="table table-hover align-middle mb-0">
                                <thead className="table-light">
                                    <tr>
                                        <th>Paciente</th>
                                        <th>Doctor</th>
                                        <th>Fecha y Hora</th>
                                        <th>Motivo</th>
                                        <th>Estado</th>
                                        <th className="text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {citasFiltradas.map(cita => (
                                        <tr key={cita.id}>
                                            <td>{cita.paciente?.nombreCompleto ?? '—'}</td>
                                            <td>{cita.doctor?.nombreCompleto ?? '—'}</td>
                                            <td>{new Date(cita.fechaHora).toLocaleString('es-MX')}</td>
                                            <td style={{ maxWidth: '200px' }} className="text-truncate">
                                                {cita.motivo}
                                            </td>
                                            <td>
                                                <span className={badgeEstado(cita.estado)}>
                                                    {textoEstado[cita.estado]}
                                                </span>
                                            </td>
                                            <td className="text-center">
                                                <div className="d-flex justify-content-center gap-1">
                                                    <button
                                                        onClick={() => onDetalle(cita)}
                                                        title="Ver detalle"
                                                        className="btn btn-sm btn-info text-white">
                                                        <i className="bi bi-eye"></i>
                                                    </button>
                                                    <button
                                                        onClick={() => onEditar(cita)}
                                                        title="Editar"
                                                        className="btn btn-sm btn-warning">
                                                        <i className="bi bi-pencil"></i>
                                                    </button>
                                                    {cita.estado !== EstadoCita.Cancelada && (
                                                        <button
                                                            onClick={() => setCitaACancelar(cita.id)}
                                                            title="Cancelar"
                                                            className="btn btn-sm btn-danger">
                                                            <i className="bi bi-x-circle"></i>
                                                        </button>
                                                    )}
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>

                        {citaACancelar !== null && (
                            <ModalConfirmacion
                                titulo="Cancelar Cita"
                                mensaje="¿Estás seguro de que deseas cancelar esta cita? Esta acción no se puede deshacer."
                                onConfirmar={handleCancelar}
                                onCancelar={() => setCitaACancelar(null)}
                            />
                        )}
                    </div>
                )}
            </div>
        </>
    );
};

export default ListaCitas;