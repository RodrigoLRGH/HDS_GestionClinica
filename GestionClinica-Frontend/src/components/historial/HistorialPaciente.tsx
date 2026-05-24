import { useState } from 'react';
import { ClipboardList, User, Plus, ChevronDown } from 'lucide-react';
import { type Paciente } from '../../types/Paciente/Paciente';
import { usePacientes } from '../../hooks/usePacientes';
import { useEvoluciones } from '../../hooks/useEvoluciones';
import ListaEvoluciones from './ListaEvoluciones';
import NuevaEvolucion from './NuevaEvolucion';
import LoadingSpinner from '../common/LoadingSpinner';
import ErrorAlert from '../common/ErrorAlert';
import { Button } from '../ui/button';

const HistorialPaciente = () => {
    const { pacientes, cargando: cargandoPacientes } = usePacientes();
    const { evoluciones, cargando, error, cargarEvolucionesPorPaciente, crear } = useEvoluciones();
    const [pacienteSeleccionado, setPacienteSeleccionado] = useState<Paciente | null>(null);
    const [mostrarFormulario, setMostrarFormulario] = useState(false);

    const handleSeleccionarPaciente = async (id: number) => {
        const paciente = pacientes.find(p => p.id === id) ?? null;
        setPacienteSeleccionado(paciente);
        setMostrarFormulario(false);
        if (id > 0) await cargarEvolucionesPorPaciente(id);
    };

    return (
        <div className="p-4">
            <div className="flex items-center gap-2 mb-6">
                <ClipboardList size={28} className="text-green-600" />
                <h2 className="text-2xl font-bold text-gray-800">Historial Clínico</h2>
            </div>

            <div className="bg-white border rounded-xl shadow-sm p-4 mb-4">
                <label className="flex items-center gap-2 text-sm font-medium mb-2 text-gray-700">
                    <User size={16} /> Seleccionar Paciente
                </label>
                {cargandoPacientes ? (
                    <LoadingSpinner />
                ) : (
                    <div className="relative w-full md:w-1/2">
                        <select
                            className="border rounded-lg px-3 py-2 w-full appearance-none focus:outline-none focus:ring-2 focus:ring-green-300"
                            onChange={e => handleSeleccionarPaciente(Number(e.target.value))}>
                            <option value={0}>Seleccione un paciente...</option>
                            {pacientes.map(p => (
                                <option key={p.id} value={p.id}>{p.nombreCompleto}</option>
                            ))}
                        </select>
                        <ChevronDown size={16} className="absolute right-3 top-3 text-gray-400 pointer-events-none" />
                    </div>
                )}
            </div>

            {error && <ErrorAlert mensaje={error} />}

            {pacienteSeleccionado && (
                <div>
                    <div className="flex justify-between items-center mb-4">
                        <div>
                            <h3 className="text-lg font-semibold text-gray-700">
                                Evoluciones de{' '}
                                <span className="text-green-600">{pacienteSeleccionado.nombreCompleto}</span>
                            </h3>
                            <p className="text-sm text-gray-400">
                                {evoluciones.length} {evoluciones.length === 1 ? 'registro' : 'registros'}
                            </p>
                        </div>
                        <Button
                            onClick={() => setMostrarFormulario(true)}
                            className="flex items-center gap-2 bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700">
                            <Plus size={18} /> Nueva Evolución
                        </Button>
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

                    {cargando ? (
                        <LoadingSpinner />
                    ) : (
                        <ListaEvoluciones evoluciones={evoluciones} />
                    )}
                </div>
            )}

            {!pacienteSeleccionado && !cargandoPacientes && (
                <div className="text-center text-gray-400 py-16">
                    <ClipboardList size={56} className="mx-auto mb-3 opacity-20" />
                    <p className="text-lg">Selecciona un paciente para ver su historial</p>
                </div>
            )}
        </div>
    );
};

export default HistorialPaciente;