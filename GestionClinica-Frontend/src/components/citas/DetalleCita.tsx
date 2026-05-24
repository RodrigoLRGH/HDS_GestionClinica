import { User, Stethoscope, Clock, Info, FileText, StickyNote, ArrowLeft, Pencil } from 'lucide-react';
import { type Cita } from '../../types/Cita/Cita';
import { EstadoCita } from '../../types/Enumeraciones/EstadoCita';
import { Button } from '../ui/button';

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
        const colores: Record<number, string> = {
            1: 'bg-yellow-100 text-yellow-700',
            2: 'bg-blue-100 text-blue-700',
            3: 'bg-red-100 text-red-700',
            4: 'bg-green-100 text-green-700',
            5: 'bg-gray-100 text-gray-700',
        };
        return `px-3 py-1 rounded-full text-xs font-semibold ${colores[estado] ?? 'bg-gray-100'}`;
    };

    return (
        <div className="p-4 max-w-2xl mx-auto">
            <div className="flex items-center gap-2 mb-6">
                <FileText className="text-blue-600" size={28} />
                <h2 className="text-2xl font-bold text-gray-800">Detalle de Cita</h2>
            </div>

            <div className="bg-white border rounded-xl shadow-md overflow-hidden">

                <div className="bg-blue-500 border-b px-6 py-3 flex justify-between items-center">
                    <span className="text-sm text-white font-medium">Cita #{cita.id}</span>
                </div>

                <div className="p-6 space-y-4">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">

                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3">
                            <User className=" mt-1" size={20} />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase">Paciente</p>
                                <p className="font-semibold text-gray-800">
                                    {cita.paciente?.nombreCompleto ?? '—'}
                                </p>
                            </div>
                        </div>

                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3">
                            <Stethoscope className=" mt-1" size={20} />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase">Doctor</p>
                                <p className="font-semibold text-gray-800">
                                    {cita.doctor?.nombreCompleto ?? '—'}
                                </p>
                            </div>
                        </div>

                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3">
                            <Clock className=" mt-1" size={20} />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase">Fecha y Hora</p>
                                <p className="font-semibold text-gray-800">
                                    {new Date(cita.fechaHora).toLocaleString('es-MX')}
                                </p>
                            </div>
                        </div>

                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3">
                            <Info className=" mt-1" size={20} />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase">Estado</p>
                                <span className={badgeEstado(cita.estado)}>
                                    {textoEstado[cita.estado]}
                                </span>
                            </div>
                        </div>

                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3 col-span-2">
                            <FileText className=" mt-1" size={20} />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase">Motivo</p>
                                <p className="font-semibold text-gray-800">{cita.motivo}</p>
                            </div>
                        </div>

                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3 col-span-2">
                            <StickyNote className=" mt-1" size={20} />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase">Notas</p>
                                <p className="text-gray-600">{cita.notas ?? '—'}</p>
                            </div>
                        </div>
                    </div>

                    <div className="flex justify-end gap-2 pt-2 border-t">
                        <Button
                            onClick={onVolver}
                            className="flex items-center gap-2 bg-gray-100 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-200">
                            <ArrowLeft size={16} /> Volver
                        </Button>
                        {cita.estado !== EstadoCita.Cancelada && (
                            <Button
                                onClick={onEditar}
                                className="flex items-center gap-2 bg-yellow-500 text-white px-4 py-2 rounded-lg hover:bg-yellow-600">
                                <Pencil size={16} /> Editar
                            </Button>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default DetalleCita;