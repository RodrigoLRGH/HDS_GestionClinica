import { useState, useEffect } from 'react';
import { type Cita } from '../../types/Cita/Cita';
import { usePacientes } from '../../hooks/usePacientes';
import { useDoctores } from '../../hooks/useDoctores';
import ErrorAlert from '../common/ErrorAlert';
import type { ActualizarCitaDTO } from '../../types/Cita/ActualizarCitaDTO';
import type { CrearCitaDTO } from '../../types/Cita/CrearCitaDTO';
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
            setForm({
                idPaciente: citaEditar.idPaciente,
                idDoctor: citaEditar.idDoctor,
                fechaHora: new Date(citaEditar.fechaHora).toISOString().slice(0, 16),
                motivo: citaEditar.motivo,
                notas: citaEditar.notas ?? '',
                estado: citaEditar.estado
            });
        }
    }, [citaEditar]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const errorValidacion = validarCita(form);
        if (errorValidacion) { setError(errorValidacion); return; }

        setGuardando(true);
        const dto = citaEditar
            ? { ...form, id: citaEditar.id } as ActualizarCitaDTO
            : form as CrearCitaDTO;

        const exito = await onGuardar(dto);
        if (!exito) setError('Error al guardar la cita');
        setGuardando(false);
    };

    return (
        <>
            <div className="container py-4" style={{ maxWidth: '720px' }}>
                <div className="d-flex align-items-center gap-2 mb-4">
                    <i className="bi bi-clipboard-check fs-3 text-primary"></i>
                    <h2 className="fw-bold mb-0">
                        {citaEditar ? 'Editar Cita' : 'Nueva Cita'}
                    </h2>
                </div>

                {error && <ErrorAlert mensaje={error} onCerrar={() => setError(null)} />}

                <div className="card shadow-sm rounded-3">
                    <div className="card-body p-4">
                        <form onSubmit={handleSubmit}>

                            <div className="row g-3 mb-3">
                                <div className="col-12 col-md-6">
                                    <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                        <i className="bi bi-person"></i> Paciente
                                    </label>
                                    <select
                                        className="form-select"
                                        value={form.idPaciente}
                                        onChange={e => setForm({ ...form, idPaciente: Number(e.target.value) })}>
                                        <option value={0}>Seleccione...</option>
                                        {pacientes.map(p => (
                                            <option key={p.id} value={p.id}>{p.nombreCompleto}</option>
                                        ))}
                                    </select>
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
                                                {d.nombreCompleto}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                            </div>

                            <div className="mb-3">
                                <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                    <i className="bi bi-clock"></i> Fecha y Hora
                                </label>
                                <input
                                    type="datetime-local"
                                    className="form-control"
                                    value={form.fechaHora}
                                    onChange={e => setForm({ ...form, fechaHora: e.target.value })}
                                />
                            </div>

                            <div className="mb-3">
                                <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                    <i className="bi bi-file-text"></i> Motivo
                                </label>
                                <textarea
                                    className="form-control"
                                    rows={3}
                                    maxLength={500}
                                    value={form.motivo}
                                    onChange={e => setForm({ ...form, motivo: e.target.value })}
                                />
                                <div className={`form-text ${form.motivo.length >= 450 ? 'text-danger' : 'text-muted'}`}>
                                    {form.motivo.length} / 500
                                </div>
                            </div>

                            <div className="mb-3">
                                <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                    <i className="bi bi-sticky"></i> Notas
                                    <span className="text-muted fw-normal">(opcional)</span>
                                </label>
                                <textarea
                                    className="form-control"
                                    rows={2}
                                    value={form.notas}
                                    onChange={e => setForm({ ...form, notas: e.target.value })}
                                />
                            </div>

                            <div className="mb-4">
                                <label className="form-label d-flex align-items-center gap-2 fw-medium">
                                    <i className="bi bi-clipboard-check"></i> Estado
                                </label>
                                <select
                                    className="form-select"
                                    value={form.estado}
                                    onChange={e => setForm({ ...form, estado: Number(e.target.value) })}>
                                    <option value={1}>Pendiente</option>
                                    <option value={2}>Confirmada</option>
                                    <option value={3}>Cancelada</option>
                                    <option value={4}>Completada</option>
                                    <option value={5}>No Asistió</option>
                                </select>
                            </div>

                            <div className="d-flex justify-content-end gap-2 pt-3 border-top">
                                <button
                                    type="button"
                                    onClick={onCancelar}
                                    className="btn btn-outline-danger d-flex align-items-center gap-2">
                                    <i className="bi bi-x-lg"></i> Cancelar
                                </button>
                                <button
                                    type="submit"
                                    disabled={guardando}
                                    className="btn btn-primary d-flex align-items-center gap-2">
                                    <i className="bi bi-floppy"></i>
                                    {guardando ? 'Guardando...' : 'Guardar'}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </>
    );
};

export default FormularioCita;