import { useState } from 'react';
import { type Paciente } from '../../types/Paciente/Paciente';
import { usePacientes } from '../../hooks/usePacientes';
import { useEvoluciones } from '../../hooks/useEvoluciones';
import ListaEvoluciones from './ListaEvoluciones';
import NuevaEvolucion from './NuevaEvolucion';
import LoadingSpinner from '../common/LoadingSpinner';
import ErrorAlert from '../common/ErrorAlert';

const HistorialPaciente = () => {
    const { pacientes, cargando: cargandoPacientes } = usePacientes();
    const { evoluciones, cargando, error, cargarEvolucionesPorPaciente, crear } = useEvoluciones();
    const [pacienteSeleccionado, setPacienteSeleccionado] = useState<Paciente | null>(null);
    const [mostrarFormulario, setMostrarFormulario] = useState(false);

    const handleSeleccionarPaciente = async (id: number) => {
        const paciente = pacientes.find(p => p.id === id) ?? null;
        setPacienteSeleccionado(paciente);
        setMostrarFormulario(false);
        if (id > 0)
            await cargarEvolucionesPorPaciente(id);
    };

    return (
        <>
            <div className="container-fluid p-4">
                <div className="d-flex align-items-center gap-2 mb-4">
                    <i className="bi bi-clipboard-check fs-3 text-success"></i>
                    <h2 className="fw-bold mb-0">Historial Clínico</h2>
                </div>

                <div className="card shadow-sm rounded-3 mb-4">
                    <div className="card-body p-4">
                        <label className="form-label d-flex align-items-center gap-2 fw-medium">
                            <i className="bi bi-person"></i> Seleccionar Paciente
                        </label>
                        {cargandoPacientes ? (
                            <LoadingSpinner />
                        ) : (
                            <div className="col-12 col-md-6">
                                <select
                                    className="form-select"
                                    onChange={e => handleSeleccionarPaciente(Number(e.target.value))}>
                                    <option value={0}>Seleccione un paciente...</option>
                                    {pacientes.map(p => (
                                        <option key={p.id} value={p.id}>{p.nombreCompleto}</option>
                                    ))}
                                </select>
                            </div>
                        )}
                    </div>
                </div>

                {error && <ErrorAlert mensaje={error} />}

                {pacienteSeleccionado && (
                    <div>
                        <div className="d-flex justify-content-between align-items-center mb-4">
                            <div>
                                <h5 className="fw-semibold text-muted mb-0">
                                    Evoluciones de{' '}
                                    <span className="text-success">{pacienteSeleccionado.nombreCompleto}</span>
                                </h5>
                                <small className="text-muted">
                                    {evoluciones.length} {evoluciones.length === 1 ? 'registro' : 'registros'}
                                </small>
                            </div>
                            <button
                                onClick={() => setMostrarFormulario(true)}
                                className="btn btn-success d-flex align-items-center gap-2">
                                <i className="bi bi-plus-lg"></i> Nueva Evolución
                            </button>
                        </div>

                        {mostrarFormulario && (
                            <NuevaEvolucion
                                idPaciente={pacienteSeleccionado.id}
                                onGuardar={async (dto) => {
                                    const exito = await crear(dto) ?? false;
                                    if (exito) {
                                        setMostrarFormulario(false);
                                        await cargarEvolucionesPorPaciente(pacienteSeleccionado.id);
                                    }
                                    return exito;
                                }}
                                onCancelar={() => setMostrarFormulario(false)} />
                        )}

                        {cargando ? <LoadingSpinner /> : <ListaEvoluciones evoluciones={evoluciones} />}
                    </div>
                )}

                {!pacienteSeleccionado && !cargandoPacientes && (
                    <div className="text-center text-muted py-5">
                        <i className="bi bi-clipboard display-1 opacity-25 d-block mb-3"></i>
                        <p className="fs-5">Selecciona un paciente para ver su historial</p>
                    </div>
                )}
            </div>
        </>
    );
};

export default HistorialPaciente;