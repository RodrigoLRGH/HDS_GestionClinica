import { useState } from 'react';
import { Search, Calendar, Plus, Eye, Pencil, XCircle, Filter } from 'lucide-react';
import { type Cita } from '../../types/Cita/Cita';
import { EstadoCita } from '../../types/Enumeraciones/EstadoCita';
import LoadingSpinner from '../common/LoadingSpinner';
import ErrorAlert from '../common/ErrorAlert';
import { Button } from '@/components/ui/button';
import ModalConfirmacion from '../common/ModalConfirmacion';

const textoEstado: Record<number, string> = {
    1: 'Pendiente',
    2: 'Confirmada',
    3: 'Cancelada',
    4: 'Completada',
    5: 'No Asistió'
};

const badgeEstado = (estado: number) => {
    const colores: Record<number, string> = {
        1: 'bg-yellow-400 text-yellow-900',
        2: 'bg-blue-500 text-white',
        3: 'bg-red-500 text-white',
        4: 'bg-green-500 text-white',
        5: 'bg-gray-500 text-white',
    };
    return `px-2 py-1 rounded-full text-xs font-semibold ${colores[estado] ?? 'bg-gray-500 text-white'}`;
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
            <div className="p-4">
                <div className="flex justify-between items-center mb-6">
                    <div className="flex items-center gap-2">
                        <Calendar size={28} className="text-blue-600" />
                        <h2 className="text-2xl font-bold text-gray-800">Citas Médicas</h2>
                    </div>
                    <Button
                        onClick={onNueva}
                        className="flex items-center gap-2 bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700">
                        <Plus size={18} /> Nueva Cita
                    </Button>
                </div>

                {error && <ErrorAlert mensaje={error} />}

                <div className="bg-white border rounded-xl shadow-sm p-4 mb-4">
                    <div className="flex items-center gap-2 mb-3 text-gray-600">
                        <Filter size={16} />
                        <span className="text-sm font-medium">Filtros</span>
                    </div>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                        <div className="relative">
                            <Search size={16} className="absolute left-3 top-3 text-gray-400" />
                            <input
                                type="text"
                                placeholder="Buscar por paciente..."
                                className="border rounded-lg pl-9 pr-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                value={filtroPaciente}
                                onChange={e => setFiltroPaciente(e.target.value)}
                            />
                        </div>
                        <select
                            className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                            value={filtroEstado}
                            onChange={e => setFiltroEstado(e.target.value === '' ? '' : Number(e.target.value))}>
                            <option value="">Todos los estados</option>
                            <option value={1}>Pendiente</option>
                            <option value={2}>Confirmada</option>
                            <option value={3}>Cancelada</option>
                            <option value={4}>Completada</option>
                            <option value={5}>No Asistió</option>
                        </select>
                        <div className="relative">
                            <Calendar size={16} className="absolute left-3 top-3 text-gray-400" />
                            <input
                                type="date"
                                className="border rounded-lg pl-9 pr-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                value={filtroFecha}
                                onChange={e => setFiltroFecha(e.target.value)}
                            />
                        </div>
                    </div>
                </div>

                {citasFiltradas.length === 0 ? (
                    <div className="text-center text-gray-400 py-10">
                        <Calendar size={48} className="mx-auto mb-2 opacity-30" />
                        <p>No hay citas registradas.</p>
                    </div>
                ) : (
                    <div className="bg-white border rounded-xl shadow-sm overflow-hidden">
                        <div className="overflow-x-auto">
                            <table className="w-full text-sm">
                                <thead className="bg-gray-50 text-gray-600 border-b">
                                    <tr>
                                        <th className="px-4 py-3 text-left">Paciente</th>
                                        <th className="px-4 py-3 text-left">Doctor</th>
                                        <th className="px-4 py-3 text-left">Fecha y Hora</th>
                                        <th className="px-4 py-3 text-left">Motivo</th>
                                        <th className="px-4 py-3 text-left">Estado</th>
                                        <th className="px-4 py-3 text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {citasFiltradas.map(cita => (
                                        <tr key={cita.id} className="border-t hover:bg-gray-50 transition-colors">
                                            <td className="px-4 py-3">{cita.paciente?.nombreCompleto ?? '—'}</td>
                                            <td className="px-4 py-3">{cita.doctor?.nombreCompleto ?? '—'}</td>
                                            <td className="px-4 py-3">
                                                {new Date(cita.fechaHora).toLocaleString('es-MX')}
                                            </td>
                                            <td className="px-4 py-3 max-w-xs truncate">{cita.motivo}</td>
                                            <td className="px-4 py-3">
                                                <span className={badgeEstado(cita.estado)}>
                                                    {textoEstado[cita.estado]}
                                                </span>
                                            </td>
                                            <td className="px-4 py-3 text-center">
                                                <div className="flex justify-center gap-1">
                                                    <Button
                                                        onClick={() => onDetalle(cita)}
                                                        title="Ver detalle"
                                                        className="p-1.5 rounded-lg bg-sky-500  hover:bg-sky-600">
                                                        <Eye size={16} />
                                                    </Button>
                                                    <Button
                                                        onClick={() => onEditar(cita)}
                                                        title="Editar"
                                                        className="p-1.5 rounded-lg bg-yellow-500  hover:bg-yellow-600">
                                                        <Pencil size={16} />
                                                    </Button>
                                                    {cita.estado !== EstadoCita.Cancelada && (
                                                        <Button
                                                            onClick={() => setCitaACancelar(cita.id)}
                                                            title="Cancelar"
                                                            className="p-1.5 rounded-lg bg-red-500 text-black hover:bg-red-600">
                                                            <XCircle size={16} />
                                                        </Button>
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