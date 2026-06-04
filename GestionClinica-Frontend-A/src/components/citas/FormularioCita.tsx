import { useState, useEffect } from 'react';
import { User, Stethoscope, Clock, FileText, StickyNote, Save, X, ClipboardList } from 'lucide-react';
import { type Cita } from '../../types/Cita/Cita';
import { usePacientes } from '../../hooks/usePacientes';
import { useDoctores } from '../../hooks/useDoctores';
import ErrorAlert from '../common/ErrorAlert';
import type { ActualizarCitaDTO } from '../../types/Cita/ActualizarCitaDTO';
import type { CrearCitaDTO } from '../../types/Cita/CrearCitaDTO';
import { Button } from '@/components/ui/button';
import { validarCita } from '../../utils/validacionesCitas';

interface Props {
    citaEditar?: Cita;
    onGuardar: (dto: CrearCitaDTO | ActualizarCitaDTO) => Promise<boolean>;
    onCancelar: () => void;
}

const FormularioCita = ({ citaEditar, onGuardar, onCancelar }: Props) => {
    const { pacientes } = usePacientes();
    const { doctores } = useDoctores();
    const [guardando, setGuardando] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const [form, setForm] = useState({
        idPaciente: 0,
        idDoctor: 0,
        fechaHora: '',
        motivo: '',
        notas: '',
        estado: 1
    });

    useEffect(() => {
        if (citaEditar) {
            const fecha = new Date(citaEditar.fechaHora);
            const fechaLocal = new Date(fecha.getTime() - fecha.getTimezoneOffset() * 60000)
                .toISOString()
                .slice(0, 16);

            setForm({
                idPaciente: citaEditar.idPaciente,
                idDoctor: citaEditar.idDoctor,
                fechaHora: fechaLocal,
                motivo: citaEditar.motivo,
                notas: citaEditar.notas ?? '',
                estado: citaEditar.estado
            });
        }
    }, [citaEditar]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const errorValidacion = validarCita(form);
        if (errorValidacion) { 
            setError(errorValidacion);
            return; 
        }

        setGuardando(true);
        const dto = citaEditar
            ? { ...form, id: citaEditar.id } as ActualizarCitaDTO
            : form as CrearCitaDTO;

        const exito = await onGuardar(dto);
        if (!exito) 
            setError('Error al guardar la cita');
        
        setGuardando(false);
    };

    return (
        <>
            <div className="p-4 max-w-2xl mx-auto">
                <div className="flex items-center gap-2 mb-6">
                    <ClipboardList size={28} className="text-blue-600" />
                    <h2 className="text-2xl font-bold text-gray-800">
                        {citaEditar ? 'Editar Cita' : 'Nueva Cita'}
                    </h2>
                </div>

                {error && <ErrorAlert mensaje={error} onCerrar={() => setError(null)} />}

                <div className="bg-white border rounded-xl shadow-md p-6">
                    <form onSubmit={handleSubmit} className="space-y-4">

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div>
                                <label className="flex items-center gap-2 text-sm font-medium mb-1">
                                    <User size={16} /> Paciente
                                </label>
                                <select
                                    className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                    value={form.idPaciente}
                                    onChange={e => setForm({ ...form, idPaciente: Number(e.target.value) })}>
                                    <option value={0}>Seleccione...</option>
                                    {pacientes.map(p => (
                                        <option key={p.id} value={p.id}>{p.nombres} {p.apellidos}</option>
                                    ))}
                                </select>
                            </div>

                            <div>
                                <label className="flex items-center gap-2 text-sm font-medium mb-1">
                                    <Stethoscope size={16} /> Doctor
                                </label>
                                <select
                                    className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                    value={form.idDoctor}
                                    onChange={e => setForm({ ...form, idDoctor: Number(e.target.value) })}>
                                    <option value={0}>Seleccione...</option>
                                    {doctores.map(d => (
                                        <option key={d.id} value={d.id}>
                                            {d.nombreCompleto}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        </div>

                        <div>
                            <label className="flex items-center gap-2 text-sm font-medium mb-1">
                                <Clock size={16} /> Fecha y Hora
                            </label>
                            <input
                                type="datetime-local"
                                className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                value={form.fechaHora}
                                onChange={e => setForm({ ...form, fechaHora: e.target.value })}
                            />
                        </div>

                        <div>
                            <label className="flex items-center gap-2 text-sm font-medium mb-1">
                                <FileText size={16} /> Motivo
                            </label>
                            <textarea
                                className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                rows={3}
                                maxLength={500}
                                value={form.motivo}
                                onChange={e => setForm({ ...form, motivo: e.target.value })}
                            />
                            <p className={`${form.motivo.length >= 450 ? 'text-red-400' : 'text-gray-400'}`}>
                                {form.motivo.length} / 500
                            </p>
                        </div>

                        <div>
                            <label className="flex items-center gap-2 text-sm font-medium mb-1">
                                <StickyNote size={16} /> Notas
                                <span className="text-gray-400 font-normal">(opcional)</span>
                            </label>
                            <textarea
                                className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                rows={2}
                                value={form.notas}
                                onChange={e => setForm({ ...form, notas: e.target.value })}
                            />
                        </div>

                        <div>
                            <label className="flex items-center gap-2 text-sm font-medium mb-1">
                                <ClipboardList size={16} /> Estado
                            </label>
                            <select
                                className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-300"
                                value={form.estado}
                                onChange={e => setForm({ ...form, estado: Number(e.target.value) })}>
                                <option value={1}>Pendiente</option>
                                <option value={2}>Confirmada</option>
                                <option value={3}>Cancelada</option>
                                <option value={4}>Completada</option>
                                <option value={5}>No Asistió</option>
                            </select>
                        </div>

                        <div className="flex justify-end gap-2 pt-2 border-t">
                            <Button
                                type="button"
                                onClick={onCancelar}
                                variant="outline"
                                className="bg-red-200 text-gray-600 hover:text-gray-800 hover:cursor-pointer">
                                <X size={16} /> Cancelar
                            </Button>
                            <Button
                                type="submit"
                                disabled={guardando}
                                variant="outline"
                                className="bg-blue-500 text-white hover:bg-blue-600 hover:cursor-pointer">
                                <Save size={16} />
                                {guardando ? 'Guardando...' : 'Guardar'}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
};

export default FormularioCita;