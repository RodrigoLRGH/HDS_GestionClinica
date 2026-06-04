import { useState, useEffect } from 'react';
import { Calendar, Stethoscope, FileText, Pill, StickyNote, Save, X, Plus } from 'lucide-react';
import { type CrearEvolucionDTO } from '../../types/Evolucion/CrearEvolucionDTO';
import ErrorAlert from '../common/ErrorAlert';
import { useDoctores } from '../../hooks/useDoctores';
import { Button } from '../ui/button';
import { validarEvolucion } from '../../utils/validacionesEvoluciones';
import { obtenerHistoriaClinicaPorPaciente } from '../../api/historialApi';

interface Props {
    idPaciente: number;
    onGuardar: (dto: CrearEvolucionDTO) => Promise<boolean>;
    onCancelar: () => void;
}

const NuevaEvolucion = ({ idPaciente, onGuardar, onCancelar }: Props) => {
    const [guardando, setGuardando] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const { doctores } = useDoctores();

    const [form, setForm] = useState<CrearEvolucionDTO>({
        idHistoriaClinica: 0,
        idDoctor: 0,
        fecha: new Date().toISOString().slice(0, 10),
        diagnostico: '',
        tratamiento: '',
        notas: ''
    });

    useEffect(() => {
        const cargar = async () => {
            try {
                const res = await obtenerHistoriaClinicaPorPaciente(idPaciente);
                setForm(prev => ({ ...prev, idHistoriaClinica: res.data.id }));
            } catch {
                setError('No se encontró historia clínica para este paciente');
            }
        };
        cargar();
    }, [idPaciente]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const errorValidacion = validarEvolucion(form);
        if (errorValidacion) { setError(errorValidacion); return; }
        setGuardando(true);
        const exito = await onGuardar(form);
        if (!exito) setError('Error al guardar la evolución');
        setGuardando(false);
    };

    return (
        <>
            <div className="bg-white border rounded-xl shadow-sm overflow-hidden mb-4">
                <div className="bg-green-50 border-b px-4 py-3 flex items-center gap-2">
                    <Plus size={18} className="text-green-600" />
                    <h3 className="text-lg font-semibold text-gray-700">Nueva Evolución</h3>
                </div>

                <div className="p-4">
                    {error && <ErrorAlert mensaje={error} onCerrar={() => setError(null)} />}

                    <form onSubmit={handleSubmit} className="space-y-4">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div>
                                <label className="flex items-center gap-2 text-sm font-medium mb-1 text-gray-700">
                                    <Calendar size={16} /> Fecha
                                </label>
                                <input
                                    type="date"
                                    className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-green-300"
                                    value={form.fecha}
                                    max={new Date().toISOString().slice(0, 10)}
                                    onChange={e => setForm({ ...form, fecha: e.target.value })} />
                            </div>

                            <div>
                                <label className="flex items-center gap-2 text-sm font-medium mb-1 text-gray-700">
                                    <Stethoscope size={16} /> Doctor
                                </label>
                                <select
                                    className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-green-300"
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
                            <label className="flex items-center gap-2 text-sm font-medium mb-1 text-gray-700">
                                <FileText size={16} /> Diagnóstico
                            </label>
                            <textarea
                                className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-green-300"
                                rows={3}
                                maxLength={500}
                                value={form.diagnostico}
                                onChange={e => setForm({ ...form, diagnostico: e.target.value })} />
                            <p className={`${form.diagnostico.length >= 450 ? 'text-red-400' : 'text-gray-400'}`}>
                                {form.diagnostico.length} / 500
                            </p>
                        </div>

                        <div>
                            <label className="flex items-center gap-2 text-sm font-medium mb-1 text-gray-700">
                                <Pill size={16} /> Tratamiento
                            </label>
                            <textarea
                                className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-green-300"
                                rows={3}
                                maxLength={500}
                                value={form.tratamiento}
                                onChange={e => setForm({ ...form, tratamiento: e.target.value })} />
                            <p className={`${form.tratamiento.length >= 450 ? 'text-red-400' : 'text-gray-400'}`}>
                                {form.tratamiento.length} / 500
                            </p>
                        </div>

                        <div>
                            <label className="flex items-center gap-2 text-sm font-medium mb-1 text-gray-700">
                                <StickyNote size={16} /> Notas
                                <span className="text-gray-400 font-normal">(opcional)</span>
                            </label>
                            <textarea
                                className="border rounded-lg px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-green-300"
                                rows={2}
                                maxLength={1000}
                                value={form.notas}
                                onChange={e => setForm({ ...form, notas: e.target.value })} />
                            <p className={`${(form.notas?.length ?? 0) >= 950 ? 'text-red-400' : 'text-gray-400'}`}>
                                {form.notas?.length ?? 0} / 1000
                            </p>
                        </div>

                        <div className="flex justify-end gap-2 pt-2 border-t">
                            <Button
                                type="button"
                                onClick={onCancelar}
                                className="flex items-center gap-2 bg-gray-100 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-200">
                                <X size={16} /> Cancelar
                            </Button>
                            <Button
                                type="submit"
                                disabled={guardando}
                                className="flex items-center gap-2 bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 disabled:opacity-50">
                                <Save size={16} />
                                {guardando ? 'Guardando...' : 'Guardar Evolución'}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
};

export default NuevaEvolucion;