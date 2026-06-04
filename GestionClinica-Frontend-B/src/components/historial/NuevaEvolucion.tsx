import { useState, useEffect } from 'react';
import { type CrearEvolucionDTO } from '../../types/Evolucion/CrearEvolucionDTO';
import { obtenerHistorialPorPaciente } from '../../api/historialApi';
import ErrorAlert from '../common/ErrorAlert';
import { useDoctores } from '../../hooks/useDoctores';
import { validarEvolucion } from '../../utils/validacionesEvoluciones';

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
                const res = await obtenerHistorialPorPaciente(idPaciente);
                if (res.data.length > 0) {
                    setForm(prev => ({ ...prev, idHistoriaClinica: res.data[0].idHistoriaClinica }));
                }
            } catch {
                setError('No se encontró historia clínica para este paciente');
            }
        };
        cargar();
    }, [idPaciente]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const errorValidacion = validarEvolucion(form);
        if (errorValidacion) {
            setError(errorValidacion);
            return;
        }
        setGuardando(true);
        const exito = await onGuardar(form);
        if (!exito)
            setError('Error al guardar la evolución');
        setGuardando(false);
    };

    return (
        <>
            <div className="card shadow-sm rounded-3 overflow-hidden mb-4">
                <div className="card-header bg-success bg-opacity-10 d-flex align-items-center gap-2">
                    <i className="bi bi-plus-lg text-success"></i>
                    <h5 className="fw-semibold text-muted mb-0">Nueva Evolución</h5>
                </div>

                <div className="card-body p-4">
                    {error && <ErrorAlert mensaje={error} onCerrar={() => setError(null)} />}

                    <form onSubmit={handleSubmit}>
                        <div className="row g-3 mb-3">
                            <div className="col-12 col-md-6">
                                <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                    <i className="bi bi-calendar"></i> Fecha
                                </label>
                                <input
                                    type="date"
                                    className="form-control"
                                    value={form.fecha}
                                    max={new Date().toISOString().slice(0, 10)}
                                    onChange={e => setForm({ ...form, fecha: e.target.value })} />
                            </div>

                            <div className="col-12 col-md-6">
                                <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                    <i className="bi bi-heart-pulse"></i> Doctor
                                </label>
                                <select
                                    className="form-select"
                                    value={form.idDoctor}
                                    onChange={e => setForm({ ...form, idDoctor: Number(e.target.value) })}>
                                    <option value={0}>Seleccione...</option>
                                    {doctores.map(d => (
                                        <option key={d.id} value={d.id}>
                                            {d.nombreCompleto} - {d.especialidad?.nombre}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        </div>

                        <div className="mb-3">
                            <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                <i className="bi bi-file-text"></i> Diagnóstico
                            </label>
                            <textarea
                                className="form-control"
                                rows={3}
                                maxLength={500}
                                value={form.diagnostico}
                                onChange={e => setForm({ ...form, diagnostico: e.target.value })} />
                            <div className={`form-text ${form.diagnostico.length >= 450 ? 'text-danger' : 'text-muted'}`}>
                                {form.diagnostico.length} / 500
                            </div>
                        </div>

                        <div className="mb-3">
                            <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                <i className="bi bi-capsule"></i> Tratamiento
                            </label>
                            <textarea
                                className="form-control"
                                rows={3}
                                maxLength={500}
                                value={form.tratamiento}
                                onChange={e => setForm({ ...form, tratamiento: e.target.value })} />
                            <div className={`form-text ${form.tratamiento.length >= 450 ? 'text-danger' : 'text-muted'}`}>
                                {form.tratamiento.length} / 500
                            </div>
                        </div>

                        <div className="mb-4">
                            <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                <i className="bi bi-sticky"></i> Notas
                                <span className="text-muted fw-normal">(opcional)</span>
                            </label>
                            <textarea
                                className="form-control"
                                rows={2}
                                maxLength={1000}
                                value={form.notas}
                                onChange={e => setForm({ ...form, notas: e.target.value })} />
                            <div className={`form-text ${(form.notas?.length ?? 0) >= 950 ? 'text-danger' : 'text-muted'}`}>
                                {form.notas?.length ?? 0} / 1000
                            </div>
                        </div>

                        <div className="d-flex justify-content-end gap-2 pt-3 border-top">
                            <button
                                type="button"
                                onClick={onCancelar}
                                className="btn btn-secondary d-flex align-items-center gap-2">
                                <i className="bi bi-x-lg"></i> Cancelar
                            </button>
                            <button
                                type="submit"
                                disabled={guardando}
                                className="btn btn-success d-flex align-items-center gap-2">
                                <i className="bi bi-floppy"></i>
                                {guardando ? 'Guardando...' : 'Guardar Evolución'}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
};

export default NuevaEvolucion;