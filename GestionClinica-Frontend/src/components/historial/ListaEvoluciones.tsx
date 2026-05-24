import { Calendar, Stethoscope, FileText, Pill, StickyNote, ClipboardList } from 'lucide-react';
import { type Evolucion } from '../../types/Evolucion/Evolucion';

interface Props {
    evoluciones: Evolucion[];
}

const ListaEvoluciones = ({ evoluciones }: Props) => {
    if (evoluciones.length === 0) {
        return (
            <div className="text-center text-gray-400 py-16">
                <ClipboardList size={56} className="mx-auto mb-3 opacity-20" />
                <p className="text-lg">No hay evoluciones registradas para este paciente.</p>
            </div>
        );
    }

    return (
        <div className="space-y-4">
            {evoluciones.map(evolucion => (
                <div key={evolucion.id} className="bg-white border rounded-xl shadow-sm overflow-hidden">
                    <div className="bg-green-50 border-b px-4 py-3 flex justify-between items-center">
                        <div className="flex items-center gap-2 text-gray-600">
                            <Calendar size={16} />
                            <span className="text-sm font-medium">
                                {new Date(evolucion.fecha).toLocaleDateString('es-MX', {
                                    year: 'numeric',
                                    month: 'long',
                                    day: 'numeric'
                                })}
                            </span>
                        </div>
                        <div className="flex items-center gap-2 text-gray-600">
                            <Stethoscope size={16} />
                            <span className="text-sm font-medium">
                                {evolucion.doctor?.nombreCompleto ?? '—'}
                            </span>
                        </div>
                    </div>

                    <div className="p-4 grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3">
                            <FileText size={18} className="mt-0.5 shrink-0 text-gray-400" />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase mb-1">
                                    Diagnóstico
                                </p>
                                <p className="text-gray-700 text-sm">{evolucion.diagnostico}</p>
                            </div>
                        </div>

                        <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3">
                            <Pill size={18} className="mt-0.5 shrink-0 text-gray-400" />
                            <div>
                                <p className="text-xs text-gray-400 font-semibold uppercase mb-1">
                                    Tratamiento
                                </p>
                                <p className="text-gray-700 text-sm">{evolucion.tratamiento}</p>
                            </div>
                        </div>

                        {evolucion.notas && (
                            <div className="flex items-start gap-3 bg-gray-50 rounded-lg p-3 col-span-2">
                                <StickyNote size={18} className="mt-0.5 shrink-0 text-gray-400" />
                                <div>
                                    <p className="text-xs text-gray-400 font-semibold uppercase mb-1">
                                        Notas
                                    </p>
                                    <p className="text-gray-600 text-sm">{evolucion.notas}</p>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            ))}
        </div>
    );
};

export default ListaEvoluciones;